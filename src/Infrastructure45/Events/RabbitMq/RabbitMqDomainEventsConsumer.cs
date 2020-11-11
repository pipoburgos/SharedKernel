using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Events;
using SharedKernel.Application.Reflection;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqDomainEventsConsumer : IDomainEventsConsumer
    {
        private DomainEventSubscribersInformation _information;
        private readonly RabbitMqConfig _config;
        private readonly DomainEventJsonDeserializer _deserializer;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, object> _domainEventSubscribers = new Dictionary<string, object>();
        private const int MaxRetries = 2;
        private const string HeaderRedelivery = "redelivery_count";

        public RabbitMqDomainEventsConsumer(
            DomainEventSubscribersInformation information,
            DomainEventJsonDeserializer deserializer,
            IServiceProvider serviceProvider,
            RabbitMqConfig config)
        {
            _information = information;
            _deserializer = deserializer;
            _serviceProvider = serviceProvider;
            _config = config;
        }

        public Task Consume(CancellationToken cancellationToken)
        {
            _information.RabbitMqFormattedNames().ForEach(queue => ConsumeMessages(queue, cancellationToken));
            return Task.CompletedTask;
        }

        public void ConsumeMessages(string queue, CancellationToken cancellationToken, ushort prefetchCount = 10)
        {
            var channel = _config.Channel();

            DeclareQueue(channel, queue);

            channel.BasicQos(0, prefetchCount, false);
            var scope = _serviceProvider.CreateScope();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = _deserializer.Deserialize(message);

                var subscriber = _domainEventSubscribers.ContainsKey(queue)
                    ? _domainEventSubscribers[queue]
                    : SubscribeFor(queue, scope);

                try
                {
                    await ((IDomainEventSubscriberBase)subscriber).On(@event, cancellationToken);
                }
                catch
                {
                    HandleConsumptionError(ea, queue);
                }

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue, false, consumer);
        }

        public void WithSubscribersInformation(DomainEventSubscribersInformation information)
        {
            _information = information;
        }

        private object SubscribeFor(string queue, IServiceScope scope)
        {
            var queueParts = queue.Split('.');
            var subscriberName = ToCamelFirstUpper(queueParts.Last());

            var t = ReflectionHelper.GetType(subscriberName);

            var subscriber = scope.ServiceProvider.GetRequiredService(t);
            _domainEventSubscribers.Add(queue, subscriber);
            return subscriber;
        }

        private string ToCamelFirstUpper(string text)
        {
            var textInfo = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).TextInfo;
            return textInfo.ToTitleCase(text).Replace("_", string.Empty);
        }

        private void DeclareQueue(IModel channel, string queue)
        {
            channel.QueueDeclare(queue,
                true,
                false,
                false
            );
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

            channel.BasicPublish(exchange,
                queue,
                properties,
                body);
        }
    }
}