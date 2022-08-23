using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharedKernel.Application.Reflection;
using SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMqEventBusConfiguration : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public RabbitMqEventBusConfiguration(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var channel = scope.ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Channel();
            var domainEventSubscriberProviderFactory = scope.ServiceProvider.GetRequiredService<IDomainEventSubscriberProviderFactory>();

            var exchangeName = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqConfigParams>>().Value.ExchangeName;

            var retryDomainEventExchange = RabbitMqExchangeNameFormatter.Retry(exchangeName);
            var deadLetterDomainEventExchange = RabbitMqExchangeNameFormatter.DeadLetter(exchangeName);

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            channel.ExchangeDeclare(retryDomainEventExchange, ExchangeType.Topic);
            channel.ExchangeDeclare(deadLetterDomainEventExchange, ExchangeType.Topic);

            foreach (var subscriberInformation in domainEventSubscriberProviderFactory.GetAll())
            {
                var domainEventsQueueName = RabbitMqQueueNameFormatter.Format(subscriberInformation);
                var retryQueueName = RabbitMqQueueNameFormatter.FormatRetry(subscriberInformation);
                var deadLetterQueueName = RabbitMqQueueNameFormatter.FormatDeadLetter(subscriberInformation);
                var subscribedEvent = EventNameSubscribed(subscriberInformation.SubscribedEvent);

                var queue = channel.QueueDeclare(domainEventsQueueName, true, false, false);

                var retryQueue = channel.QueueDeclare(retryQueueName, true, false, false,
                    RetryQueueArguments(exchangeName, domainEventsQueueName));

                var deadLetterQueue = channel.QueueDeclare(deadLetterQueueName, true, false, false);

                channel.QueueBind(queue, exchangeName, domainEventsQueueName);
                channel.QueueBind(retryQueue, retryDomainEventExchange, domainEventsQueueName);
                channel.QueueBind(deadLetterQueue, deadLetterDomainEventExchange, domainEventsQueueName);
                channel.QueueBind(queue, exchangeName, subscribedEvent);
            }

            return scope.ServiceProvider.GetRequiredService<RabbitMqDomainEventsConsumer>().Consume();
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

        private string EventNameSubscribed(Type subscribedEvent)
        {
            var domainEvent = ReflectionHelper.CreateInstance<dynamic>(subscribedEvent);
            return domainEvent.GetEventName();
        }
    }
}