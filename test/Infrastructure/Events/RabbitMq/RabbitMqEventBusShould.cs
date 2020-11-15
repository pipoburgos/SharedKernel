using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SharedKernel.Application.Reflection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.Validators;
using SharedKernel.Integration.Tests.Shared;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.RabbitMq
{
    public class RabbitMqEventBusShould : InfrastructureTestCase
    {
        private readonly RabbitMqEventBus _bus;
        private readonly RabbitMqDomainEventsConsumer _consumer;
        //private readonly TestAllWorksOnRabbitMqEventsPublished _subscriber;
        private const string TestDomainEvents = "test_domain_events";

        protected override string GetJsonFile()
        {
            return "Events/RabbitMq/appsettings.rabbitMq.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>));
            services.AddRabbitMqEventBus(Configuration);
            services.AddSingleton<SingletonValueContainer>();
            services.AddScoped(p =>
            {
                var executeMiddlewaresService = p.GetRequiredService<ExecuteMiddlewaresService>();
                var publisher = p.GetRequiredService<RabbitMqPublisher>();
                // var failOverBus = p.GetRequiredService<MsSqlEventBus>();
                return new RabbitMqEventBus(executeMiddlewaresService, publisher, "test_domain_events");
            });

            services.AddScoped<TestAllWorksOnRabbitMqEventsPublishedSubscriber, TestAllWorksOnRabbitMqEventsPublishedSubscriber>();
            services.AddDomainEventsSubscribers(typeof(TestAllWorksOnRabbitMqEventsPublishedSubscriber).Assembly);
            services.AddDomainEventSubscribersInformation();
            return services;
        }

        public RabbitMqEventBusShould()
        {
            _bus = GetService<RabbitMqEventBus>();
            _consumer = GetService<RabbitMqDomainEventsConsumer>();
            var config = GetService<RabbitMqConfig>();

            var channel = config.Channel();

            var fakeSubscriber = FakeSubscriber();

            CleanEnvironment(channel, fakeSubscriber);
            channel.ExchangeDeclare(TestDomainEvents, ExchangeType.Topic);
            CreateQueue(channel, fakeSubscriber);

            _consumer.WithSubscribersInformation(fakeSubscriber);
        }

        [Fact]
        public async Task PublishDomainEventFromRabbitMq()
        {
            var userId = Guid.NewGuid();
            const string name = "Name";

            var domainEvent = new UserCreated(userId, name, userId.ToString());

            await _bus.Publish(new List<DomainEvent> { domainEvent }, CancellationToken.None).ConfigureAwait(false);

            await _consumer.Consume(CancellationToken.None).ConfigureAwait(false);

            await Task.Delay(1_000, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(userId, GetService<SingletonValueContainer>().UserId);
        }

        private static DomainEventSubscribersInformation FakeSubscriber()
        {
            return new DomainEventSubscribersInformation(
                new Dictionary<Type, DomainEventSubscriberInformation>
                {
                    {
                        typeof(TestAllWorksOnRabbitMqEventsPublishedSubscriber),
                        new DomainEventSubscriberInformation(
                            typeof(TestAllWorksOnRabbitMqEventsPublishedSubscriber),
                            typeof(UserCreated)
                        )
                    }
                });
        }

        private static void CreateQueue(IModel channel,
            DomainEventSubscribersInformation domainEventSubscribersInformation)
        {
            foreach (var subscriberInformation in domainEventSubscribersInformation.All())
            {
                var domainEventsQueueName = RabbitMqQueueNameFormatter.Format(subscriberInformation);
                var queue = channel.QueueDeclare(domainEventsQueueName, true, false, false);
                var domainEvent = ReflectionHelper.CreateInstance<dynamic>(subscriberInformation.SubscribedEvent);
                channel.QueueBind(queue, TestDomainEvents, (string)domainEvent?.GetEventName());
            }
        }

        private static void CleanEnvironment(IModel channel, DomainEventSubscribersInformation information)
        {
            channel.ExchangeDelete(TestDomainEvents);

            foreach (var domainEventSubscriberInformation in information.All())
            {
                channel.QueueDelete(RabbitMqQueueNameFormatter.Format(domainEventSubscriberInformation));
            }
        }


    }
}