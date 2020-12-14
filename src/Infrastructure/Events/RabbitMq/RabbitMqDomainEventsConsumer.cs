using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Application.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqDomainEventsConsumer
    {
        private readonly RabbitMqConnectionFactory _config;
        private readonly IServiceScopeFactory _factory;
        private readonly ICustomLogger<RabbitMqDomainEventsConsumer> _logger;
        private readonly IOptions<RabbitMqConfigParams> _rabbitMqParams;
        private readonly DomainEventSubscribersInformation _information;
        private readonly DomainEventJsonDeserializer _deserializer;
        private const int MaxRetries = 2;
        private const string HeaderRedelivery = "redelivery_count";

        public RabbitMqDomainEventsConsumer(
            DomainEventSubscribersInformation information,
            DomainEventJsonDeserializer deserializer,
            RabbitMqConnectionFactory config,
            IServiceScopeFactory factory,
            ICustomLogger<RabbitMqDomainEventsConsumer> logger,
            IOptions<RabbitMqConfigParams> rabbitMqParams)
        {
            _information = information;
            _deserializer = deserializer;
            _config = config;
            _factory = factory;
            _logger = logger;
            _rabbitMqParams = rabbitMqParams;
        }

        public Task Consume()
        {
            _information.GetAllEventsSubscribers().ForEach(eventSubscriber => ConsumeMessages(eventSubscriber));
            return Task.CompletedTask;
        }

        private void ConsumeMessages(string eventSubscriber, ushort prefetchCount = 10)
        {
            var channel = _config.Channel();

            DeclareQueue(channel, eventSubscriber);

            channel.BasicQos(0, prefetchCount, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var @event = _deserializer.Deserialize(message);

                    await _factory.CreateScope().ServiceProvider.GetRequiredService<DomainEventMediator>()
                        .ExecuteOn(@event, eventSubscriber, CancellationToken.None);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, ex.Message);
                    HandleConsumptionError(ea, eventSubscriber);
                }

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(eventSubscriber, false, consumer);
        }

        private void DeclareQueue(IModel channel, string queue)
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
            return (int)(headers[HeaderRedelivery] ?? 0) >= MaxRetries;
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
}