using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Events;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using System.Text;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
internal class RabbitMqConsumer
{
    private readonly RabbitMqConnectionFactory _config;
    private readonly IRequestMediator _requestMediator;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;
    private readonly IRetriever _retriever;
    private const string HeaderRedelivery = "redelivery_count";

    /// <summary> . </summary>
    public RabbitMqConsumer(
        RabbitMqConnectionFactory config,
        IRequestMediator requestMediator,
        ILogger<RabbitMqConsumer> logger,
        IOptions<RabbitMqConfigParams> rabbitMqParams,
        IRetriever retriever)
    {
        _config = config;
        _requestMediator = requestMediator;
        _logger = logger;
        _rabbitMqParams = rabbitMqParams;
        _retriever = retriever;
    }

    public async Task ConsumeQueueAsync(string queue, ushort prefetchCount = 10,
        CancellationToken cancellationToken = default)
    {
        var commandRequestHandlerType = typeof(ICommandRequestHandler<>);
        const string method = nameof(ICommandRequestHandler<CommandRequest>.Handle);

        var channel = await _config.CreateChannelAsync(cancellationToken);

        await DeclareQueue(channel, queue, cancellationToken);

        await channel.BasicQosAsync(0, prefetchCount, false, cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (_requestMediator.HandlerImplemented(message, commandRequestHandlerType))
                {
                    await _requestMediator.Execute(message, commandRequestHandlerType, method, cancellationToken);
                    await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleConsumptionError(ea, queue, ExchangeType.Direct, cancellationToken);
            }
        };

        await channel.BasicConsumeAsync(queue, false, consumer, cancellationToken);
    }

    public async Task ConsumeTopicAsync(string topicName, ushort prefetchCount = 10,
        CancellationToken cancellationToken = default)
    {
        var domainEventSubscriberType = typeof(IDomainEventSubscriber<>);
        const string method = nameof(IDomainEventSubscriber<DomainEvent>.On);

        var channel = await _config.CreateChannelAsync(cancellationToken);

        await DeclareQueue(channel, topicName, cancellationToken);

        await channel.BasicQosAsync(0, prefetchCount, false, cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (_requestMediator.HandlerImplemented(message, domainEventSubscriberType))
                {
                    await _requestMediator.Execute(message, domainEventSubscriberType, method, cancellationToken);
                    await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleConsumptionError(ea, topicName, ExchangeType.Topic, cancellationToken);
            }
        };

        await channel.BasicConsumeAsync(topicName, false, consumer, cancellationToken: cancellationToken);
    }

    private static Task DeclareQueue(IChannel channel, string queue, CancellationToken cancellationToken)
    {
        return channel.QueueDeclareAsync(queue, true, false, false, cancellationToken: cancellationToken);
    }

    private Task HandleConsumptionError(BasicDeliverEventArgs ea, string queue, string exchangeType, CancellationToken cancellationToken)
    {
        return HasBeenRedeliveredTooMuch(ea.BasicProperties.Headers)
            ? SendToDeadLetter(ea, queue, exchangeType, cancellationToken)
            : SendToRetry(ea, queue, exchangeType, cancellationToken);
    }

    private bool HasBeenRedeliveredTooMuch(IDictionary<string, object?>? headers)
    {
        return headers != null && (int)(headers[HeaderRedelivery] ?? 0) >= _retriever.RetryCount;
    }

    private Task SendToRetry(BasicDeliverEventArgs ea, string queue, string exchangeType, CancellationToken cancellationToken)
    {
        return SendMessageTo(RabbitMqExchangeNameFormatter.Retry(_rabbitMqParams.Value.ExchangeName), exchangeType, ea,
            queue, cancellationToken);
    }

    private Task SendToDeadLetter(BasicDeliverEventArgs ea, string queue, string exchangeType, CancellationToken cancellationToken)
    {
        return SendMessageTo(RabbitMqExchangeNameFormatter.DeadLetter(_rabbitMqParams.Value.ExchangeName), exchangeType,
            ea, queue, cancellationToken);
    }

    private async Task SendMessageTo(string exchange, string exchangeType, BasicDeliverEventArgs ea, string queue, CancellationToken cancellationToken)
    {
        var channel = await _config.CreateChannelAsync(cancellationToken);
        await channel.ExchangeDeclareAsync(exchange, exchangeType, cancellationToken: cancellationToken);

        var properties = ea.BasicProperties;
        var headers = ea.BasicProperties.Headers;

        if (headers == null)
            return;

        var next = (int)(headers[HeaderRedelivery] ?? 0) + 1;

        headers.Remove(HeaderRedelivery);
        headers.Add(HeaderRedelivery, next);

        var basicProperties = new BasicProperties(properties);

        await channel.BasicPublishAsync(exchange, queue, true, basicProperties, ea.Body, cancellationToken);
    }
}
