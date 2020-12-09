using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Domain.Events;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqDomainEventsConsumer : IDomainEventsConsumer
    {
        private readonly RabbitMqConnectionFactory _config;
        private readonly DomainEventSubscribersInformation _information;
        private readonly DomainEventJsonDeserializer _deserializer;
        private readonly DomainEventMediator _domainEventMediator;
        private const int MaxRetries = 2;
        private const string HeaderRedelivery = "redelivery_count";

        public RabbitMqDomainEventsConsumer(
            DomainEventSubscribersInformation information,
            DomainEventJsonDeserializer deserializer,
            DomainEventMediator domainEventMediator,
            RabbitMqConnectionFactory config)
        {
            _information = information;
            _deserializer = deserializer;
            _domainEventMediator = domainEventMediator;
            _config = config;
        }

        public Task Consume(CancellationToken cancellationToken)
        {
            _information.GetAllEventsSubscribers().ForEach(eventSubscriber => ConsumeMessages(eventSubscriber, cancellationToken));
            return Task.CompletedTask;
        }

        private void ConsumeMessages(string eventSubscriber, CancellationToken cancellationToken, ushort prefetchCount = 10)
        {
            var channel = _config.Channel();

            DeclareQueue(channel, eventSubscriber);

            channel.BasicQos(0, prefetchCount, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = _deserializer.Deserialize(message);

                try
                {
                    await _domainEventMediator.ExecuteOn(@event, eventSubscriber, cancellationToken);
                }
                catch
                {
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
            SendMessageTo(RabbitMqExchangeNameFormatter.Retry("domain_events"), ea, queue);
        }

        private void SendToDeadLetter(BasicDeliverEventArgs ea, string queue)
        {
            SendMessageTo(RabbitMqExchangeNameFormatter.DeadLetter("domain_events"), ea, queue);
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