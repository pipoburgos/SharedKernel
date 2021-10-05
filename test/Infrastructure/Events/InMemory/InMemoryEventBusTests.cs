using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.InMemory
{
    public class InMemoryEventBusTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddInMemoryEventBus(Configuration)
                .AddDomainEvents(typeof(UserCreated))
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromMemory()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>(),
                GetRequiredService<PublishUserCreatedDomainEvent>(), 1_500);
        }


        [Fact]
        public async Task ExecutePararell()
        {
            var user1 = UserMother.Create();
            var user2 = UserMother.Create();
            var user3 = UserMother.Create();
            var user4 = UserMother.Create();

            var domainEvents = user1.PullDomainEvents();
            domainEvents.AddRange(user2.PullDomainEvents());
            domainEvents.AddRange(user3.PullDomainEvents());
            domainEvents.AddRange(user4.PullDomainEvents());

            var eventBus = GetRequiredService<IEventBus>();
            var tasks = new List<Task>();
            for (var i = 0; i < 250; i++)
            {
                tasks.Add(eventBus.Publish(domainEvents, CancellationToken.None));
            }

            await Task.WhenAll(tasks);
            await Task.Delay(30_000, CancellationToken.None);

            var singletonValueContainer = GetRequiredService<PublishUserCreatedDomainEvent>();
            singletonValueContainer.Total.Should().Be(1_000);

            singletonValueContainer.Users.Should().Contain(u => u == user1.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user2.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user3.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user4.Id);
        }
    }
}
