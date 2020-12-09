using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SharedKernel.Application.Reflection;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    public class RabbitMqEventBusConfiguration : BackgroundService
    {
        private readonly DomainEventSubscribersInformation _domainEventSubscribersInformation;
        private readonly RabbitMqDomainEventsConsumer _rabbitMqDomainEventsConsumer;
        private readonly RabbitMqConnectionFactory _config;

        private readonly string _domainEventExchange;

        public RabbitMqEventBusConfiguration(
            DomainEventSubscribersInformation domainEventSubscribersInformation,
            RabbitMqDomainEventsConsumer rabbitMqDomainEventsConsumer,
            RabbitMqConnectionFactory config,
            string domainEventExchange = "domain_events")
        {
            _domainEventSubscribersInformation = domainEventSubscribersInformation;
            _rabbitMqDomainEventsConsumer = rabbitMqDomainEventsConsumer;
            _config = config;
            _domainEventExchange = domainEventExchange;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = _config.Channel();

            var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(_domainEventExchange);
            var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(_domainEventExchange);

            channel.ExchangeDeclare(_domainEventExchange, ExchangeType.Topic);
            channel.ExchangeDeclare(retryDomainEventExchange, ExchangeType.Topic);
            channel.ExchangeDeclare(deadLetterDomainEventExchange, ExchangeType.Topic);

            foreach (var subscriberInformation in _domainEventSubscribersInformation.All())
            {
                var domainEventsQueueName = RabbitMqQueueNameFormatter.Format(subscriberInformation);
                var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(subscriberInformation);
                var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(subscriberInformation);
                var subscribedEvent = EventNameSubscribed(subscriberInformation);

                var queue = channel.QueueDeclare(domainEventsQueueName, true, false, false);

                var retryQueue = channel.QueueDeclare(retryQueueName, true, false, false,
                    RetryQueueArguments(_domainEventExchange, domainEventsQueueName));

                var deadLetterQueue = channel.QueueDeclare(deadLetterQueueName, true, false, false);

                channel.QueueBind(queue, _domainEventExchange, domainEventsQueueName);
                channel.QueueBind(retryQueue, retryDomainEventExchange, domainEventsQueueName);
                channel.QueueBind(deadLetterQueue, deadLetterDomainEventExchange, domainEventsQueueName);

                channel.QueueBind(queue, _domainEventExchange, subscribedEvent);
            }

            return _rabbitMqDomainEventsConsumer.Consume(stoppingToken);
        }

        private IDictionary<string, object> RetryQueueArguments(string domainEventExchange,
            string domainEventQueue)
        {
            return new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", domainEventExchange},
                {"x-dead-letter-routing-key", domainEventQueue},
                {"x-message-ttl", 1000}
            };
        }

        private string EventNameSubscribed(DomainEventSubscriberInformation subscriberInformation)
        {
            var domainEvent = ReflectionHelper.CreateInstance<dynamic>(subscriberInformation.SubscribedEvent);
            return domainEvent.GetEventName();
        }
    }
}