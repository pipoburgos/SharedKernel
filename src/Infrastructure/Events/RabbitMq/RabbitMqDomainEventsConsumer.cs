using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Application.Events;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.RabbitMq;

/// <summary>  </summary>
internal class RabbitMqDomainEventsConsumer
{
    private readonly IRequestDeserializer _requestDeserializer;
    private readonly RabbitMqConnectionFactory _config;
    private readonly IRequestMediator _requestMediator;
    private readonly ICustomLogger<RabbitMqDomainEventsConsumer> _logger;
    private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;
    private readonly IServiceProvider _serviceProvider;
    private readonly RetrieverOptions _retrieverOptions;
    private const string HeaderRedelivery = "redelivery_count";

    /// <summary>  </summary>
    public RabbitMqDomainEventsConsumer(
        IRequestDeserializer requestDeserializer,
        RabbitMqConnectionFactory config,
        IRequestMediator requestMediator,
        ICustomLogger<RabbitMqDomainEventsConsumer> logger,
        IOptions<RabbitMqConfigParams> rabbitMqParams,
        IOptionsService<RetrieverOptions> options,
        IServiceProvider serviceProvider)
    {
        _requestDeserializer = requestDeserializer;
        _config = config;
        _requestMediator = requestMediator;
        _logger = logger;
        _rabbitMqParams = rabbitMqParams;
        _serviceProvider = serviceProvider;
        _retrieverOptions = options.Value;
    }

    /// <summary>  </summary>
    public Task Consume()
    {
        var types = _serviceProvider.GetServices<IRequestType>().ToList();

        types.ForEach(a => ConsumeMessages(a.UniqueName));
        return Task.CompletedTask;
    }

    private void ConsumeMessages(string queue, ushort prefetchCount = 10)
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

                var handlerType = typeof(IDomainEventSubscriber<>).MakeGenericType(@event.GetType());

                TaskHelper.RunSync(_requestMediator.Execute(message, @event, handlerType,
                    nameof(IDomainEventSubscriber<DomainEvent>.On), CancellationToken.None));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                HandleConsumptionError(ea, queue);
            }

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(queue, false, consumer);
    }

    private static void DeclareQueue(IModel channel, string queue)
    {
        channel.QueueDeclare(queue, true, false, false);
    }

    private void HandleConsumptionError(BasicDeliverEventArgs ea, string queue)
    {
        if (HasBeenRedeliveredTooMuch(ea.BasicProperties.Headers))
            SendToDeadLetter(ea, queue);
        else
            SendToRetry(ea, queue);
    }

    private bool HasBeenRedeliveredTooMuch(IDictionary<string, object> headers)
    {
        return (int)(headers[HeaderRedelivery] ?? 0) >= _retrieverOptions.RetryCount;
    }

    private void SendToRetry(BasicDeliverEventArgs ea, string queue)
    {
        SendMessageTo(RabbitMqExchangeNameFormatter.Retry(_rabbitMqParams.Value.ExchangeName), ea, queue);
    }

    private void SendToDeadLetter(BasicDeliverEventArgs ea, string queue)
    {
        SendMessageTo(RabbitMqExchangeNameFormatter.DeadLetter(_rabbitMqParams.Value.ExchangeName), ea, queue);
    }

    private void SendMessageTo(string exchange, BasicDeliverEventArgs ea, string queue)
    {
        var channel = _config.Channel();
        channel.ExchangeDeclare(exchange, ExchangeType.Topic);

        var body = ea.Body;
        var properties = ea.BasicProperties;
        var headers = ea.BasicProperties.Headers;
        headers[HeaderRedelivery] = (int)headers[HeaderRedelivery] + 1;
        properties.Headers = headers;

        channel.BasicPublish(exchange, queue, properties, body);
    }
}
