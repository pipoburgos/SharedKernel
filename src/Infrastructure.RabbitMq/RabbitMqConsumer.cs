using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Events;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using System.Text;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary>  </summary>
internal class RabbitMqConsumer
{
    private readonly IRequestDeserializer _requestDeserializer;
    private readonly RabbitMqConnectionFactory _config;
    private readonly IRequestMediator _requestMediator;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;
    private readonly IRetriever _retriever;
    private const string HeaderRedelivery = "redelivery_count";

    /// <summary>  </summary>
    public RabbitMqConsumer(
        IRequestDeserializer requestDeserializer,
        RabbitMqConnectionFactory config,
        IRequestMediator requestMediator,
        ILogger<RabbitMqConsumer> logger,
        IOptions<RabbitMqConfigParams> rabbitMqParams,
        IRetriever retriever)
    {
        _requestDeserializer = requestDeserializer;
        _config = config;
        _requestMediator = requestMediator;
        _logger = logger;
        _rabbitMqParams = rabbitMqParams;
        _retriever = retriever;
    }

    public void ConsumeQueue(string queue, ushort prefetchCount = 10)
    {
        var channel = _config.Channel();

        DeclareQueue(channel, queue);

        channel.BasicQos(0, prefetchCount, false);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = _requestDeserializer.Deserialize(message);

                var handlerType = typeof(ICommandRequestHandler<>).MakeGenericType(@event.GetType());

                TaskHelper.RunSync(_requestMediator.Execute(message, @event, handlerType,
                    nameof(ICommandRequestHandler<CommandRequest>.Handle), CancellationToken.None));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                HandleConsumptionError(ea, queue, ExchangeType.Direct);
            }

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(queue, false, consumer);
    }

    public void ConsumeTopic(string topicName, ushort prefetchCount = 10)
    {
        var channel = _config.Channel();

        DeclareQueue(channel, topicName);

        channel.BasicQos(0, prefetchCount, false);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = _requestDeserializer.Deserialize(message);

                var handlerType = typeof(IDomainEventSubscriber<>).MakeGenericType(@event.GetType());

                TaskHelper.RunSync(_requestMediator.Execute(message, @event, handlerType,
                    nameof(IDomainEventSubscriber<DomainEvent>.On), CancellationToken.None));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                HandleConsumptionError(ea, topicName, ExchangeType.Topic);
            }

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(topicName, false, consumer);
    }

    private static void DeclareQueue(IModel channel, string queue)
    {
        channel.QueueDeclare(queue, true, false, false);
    }

    private void HandleConsumptionError(BasicDeliverEventArgs ea, string queue, string exchangeType)
    {
        if (HasBeenRedeliveredTooMuch(ea.BasicProperties.Headers))
            SendToDeadLetter(ea, queue, exchangeType);
        else
            SendToRetry(ea, queue, exchangeType);
    }

    private bool HasBeenRedeliveredTooMuch(IDictionary<string, object?> headers)
    {
        return (int)(headers[HeaderRedelivery] ?? 0) >= _retriever.RetryCount;
    }

    private void SendToRetry(BasicDeliverEventArgs ea, string queue, string exchangeType)
    {
        SendMessageTo(RabbitMqExchangeNameFormatter.Retry(_rabbitMqParams.Value.ExchangeName), exchangeType, ea, queue);
    }

    private void SendToDeadLetter(BasicDeliverEventArgs ea, string queue, string exchangeType)
    {
        SendMessageTo(RabbitMqExchangeNameFormatter.DeadLetter(_rabbitMqParams.Value.ExchangeName), exchangeType, ea,
            queue);
    }

    private void SendMessageTo(string exchange, string exchangeType, BasicDeliverEventArgs ea, string queue)
    {
        var channel = _config.Channel();
        channel.ExchangeDeclare(exchange, exchangeType);

        var body = ea.Body;
        var properties = ea.BasicProperties;
        var headers = ea.BasicProperties.Headers;
        headers[HeaderRedelivery] = (int)headers[HeaderRedelivery] + 1;
        properties.Headers = headers;

        channel.BasicPublish(exchange, queue, properties, body);
    }
}
