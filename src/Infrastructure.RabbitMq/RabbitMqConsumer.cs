using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Events;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using System.Text;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
internal sealed class RabbitMqConsumer : IDisposable, IAsyncDisposable
{
    private const string HeaderRedelivery = "redelivery_count";
    private readonly RabbitMqConnectionFactory _rabbitMqConnectionFactory;
    private readonly IRequestMediator _requestMediator;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;
    private readonly IRetriever _retriever;
    private readonly IEnumerable<IRequestType> _requestsTypes;
    private IConnection _connection = null!;
    private IChannel _channel = null!;

    /// <summary> . </summary>
    public RabbitMqConsumer(
        RabbitMqConnectionFactory rabbitMqConnectionFactory,
        IRequestMediator requestMediator,
        ILogger<RabbitMqConsumer> logger,
        IOptions<RabbitMqConfigParams> rabbitMqParams,
        IRetriever retriever,
        IEnumerable<IRequestType> requestsTypes)
    {
        _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
        _requestMediator = requestMediator;
        _logger = logger;
        _rabbitMqParams = rabbitMqParams;
        _retriever = retriever;
        _requestsTypes = requestsTypes;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = await _rabbitMqConnectionFactory.CreateConnectionAsync();

        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await Consume(true, _requestsTypes, cancellationToken);
        await Consume(false, _requestsTypes, cancellationToken);
    }

    private async Task Consume(bool isTopic, IEnumerable<IRequestType> requestsTypes, CancellationToken cancellationToken = default)
    {
        string exchangeName;
        string type;
        if (isTopic)
        {
            exchangeName = _rabbitMqParams.Value.ExchangeName;
            type = ExchangeType.Topic;
        }
        else
        {
            exchangeName = _rabbitMqParams.Value.ConsumeQueue;
            type = ExchangeType.Direct;
        }

        var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(exchangeName);
        var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(exchangeName);

        await _channel.ExchangeDeclareAsync(exchangeName, type, cancellationToken: cancellationToken);
        await _channel.ExchangeDeclareAsync(retryDomainEventExchange, type, cancellationToken: cancellationToken);
        await _channel.ExchangeDeclareAsync(deadLetterDomainEventExchange, type, cancellationToken: cancellationToken);


        foreach (var requestType in requestsTypes.Where(r => r.IsTopic == isTopic))
        {
            var domainEventsQueueName = RabbitMqQueueNameFormatter.Format(requestType);
            var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(requestType);
            var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(requestType);

            var queue = await _channel.QueueDeclareAsync(domainEventsQueueName, true, false, false, cancellationToken: cancellationToken);

            var retryQueue = await _channel.QueueDeclareAsync(retryQueueName, true, false, false,
                RetryQueueArguments(exchangeName, domainEventsQueueName), cancellationToken: cancellationToken);

            var deadLetterQueue = await _channel.QueueDeclareAsync(deadLetterQueueName, true, false, false, cancellationToken: cancellationToken);

            await _channel.QueueBindAsync(queue, exchangeName, domainEventsQueueName, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(retryQueue, retryDomainEventExchange, domainEventsQueueName, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(deadLetterQueue, deadLetterDomainEventExchange, domainEventsQueueName, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(queue, exchangeName, requestType.UniqueName, cancellationToken: cancellationToken);
            if (isTopic)
                await ConsumeTopic(requestType.UniqueName, cancellationToken: cancellationToken);
            else
                await ConsumeQueue(requestType.UniqueName, cancellationToken: cancellationToken);
        }
    }

    private static IDictionary<string, object?> RetryQueueArguments(string domainEventExchange,
        string domainEventQueue)
    {
        return new Dictionary<string, object?>
            {
                {"x-dead-letter-exchange", domainEventExchange},
                {"x-dead-letter-routing-key", domainEventQueue},
                {"x-message-ttl", 1000},
            };
    }

    private async Task ConsumeQueue(string queue, ushort prefetchCount = 10, CancellationToken cancellationToken = default)
    {
        var commandRequestHandlerType = typeof(ICommandRequestHandler<>);
        const string method = nameof(ICommandRequestHandler<CommandRequest>.Handle);

        _connection = await _rabbitMqConnectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await DeclareQueue(_channel, queue, cancellationToken);

        await _channel.BasicQosAsync(0, prefetchCount, false, cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (_requestMediator.HandlerImplemented(message, commandRequestHandlerType))
                {
                    await _requestMediator.Execute(message, commandRequestHandlerType, method, cancellationToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleConsumptionError(ea, queue, ExchangeType.Direct, cancellationToken);
            }
        };

        await _channel.BasicConsumeAsync(queue, false, consumer, cancellationToken: cancellationToken);
    }

    private async Task ConsumeTopic(string topicName, ushort prefetchCount = 10, CancellationToken cancellationToken = default)
    {
        var domainEventSubscriberType = typeof(IDomainEventSubscriber<>);
        const string method = nameof(IDomainEventSubscriber<DomainEvent>.On);

        _connection = await _rabbitMqConnectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await DeclareQueue(_channel, topicName, cancellationToken);

        await _channel.BasicQosAsync(0, prefetchCount, false, cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (_requestMediator.HandlerImplemented(message, domainEventSubscriberType))
                {
                    await _requestMediator.Execute(message, domainEventSubscriberType, method, cancellationToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
            }
            catch (AlreadyClosedException ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleConsumptionError(ea, topicName, ExchangeType.Topic, cancellationToken);
            }
        };

        await _channel.BasicConsumeAsync(topicName, false, consumer, cancellationToken: cancellationToken);
    }

    private static Task DeclareQueue(IChannel channel, string queue, CancellationToken cancellationToken = default)
    {
        return channel.QueueDeclareAsync(queue, true, false, false, cancellationToken: cancellationToken);
    }

    private Task HandleConsumptionError(BasicDeliverEventArgs ea, string queue, string exchangeType, CancellationToken cancellationToken = default)
    {
        return HasBeenRedeliveredTooMuch(ea.BasicProperties.Headers!)
            ? SendToDeadLetter(ea, queue, exchangeType, cancellationToken)
            : SendToRetry(ea, queue, exchangeType, cancellationToken);
    }

    private bool HasBeenRedeliveredTooMuch(IDictionary<string, object?> headers)
    {
        return (int)(headers[HeaderRedelivery] ?? 0) >= _retriever.RetryCount;
    }

    private Task SendToRetry(BasicDeliverEventArgs ea, string queue, string exchangeType,
        CancellationToken cancellationToken = default)
    {
        return SendMessageTo(RabbitMqExchangeNameFormatter.Retry(_rabbitMqParams.Value.ExchangeName), exchangeType, ea,
            queue, cancellationToken);
    }

    private Task SendToDeadLetter(BasicDeliverEventArgs ea, string queue, string exchangeType,
        CancellationToken cancellationToken = default)
    {
        return SendMessageTo(RabbitMqExchangeNameFormatter.DeadLetter(_rabbitMqParams.Value.ExchangeName), exchangeType,
            ea, queue, cancellationToken);
    }

    private async Task SendMessageTo(string exchange, string exchangeType, BasicDeliverEventArgs ea, string queue,
        CancellationToken cancellationToken = default)
    {
        await _channel!.ExchangeDeclareAsync(exchange, exchangeType, cancellationToken: cancellationToken);

        var body = ea.Body;
        var properties = new BasicProperties
        {
            Headers = new Dictionary<string, object?>
                {{HeaderRedelivery, (int) ea.BasicProperties.Headers![HeaderRedelivery]! + 1}},
        };

        await _channel.BasicPublishAsync(exchange, queue, false, properties, body, cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Dispose();
        }

        if (_connection.IsOpen)
            _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel.IsOpen)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection.IsOpen)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}
