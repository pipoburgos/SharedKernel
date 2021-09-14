using SharedKernel.Domain.Events;
using SharedKernel.Domain.Tests.Users;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace SharedKernel.Integration.Tests.Events
{
    public static class PublishUserCreatedDomainEventCase
    {
        public static async Task PublishDomainEvent(IEventBus eventBus, PublishUserCreatedDomainEvent singletonValueContainer, int millisecondsDelay)
        {
            var user1 = UserMother.Create();
            var user2 = UserMother.Create();
            var user3 = UserMother.Create();
            var user4 = UserMother.Create();

            var domainEvents = user1.PullDomainEvents();
            domainEvents.AddRange(user2.PullDomainEvents());
            domainEvents.AddRange(user3.PullDomainEvents());
            domainEvents.AddRange(user4.PullDomainEvents());
            await eventBus.Publish(domainEvents, CancellationToken.None);

            await Task.Delay(millisecondsDelay, CancellationToken.None);

            singletonValueContainer.Total.Should().Be(4);

            singletonValueContainer.Users.Should().Contain(u => u == user1.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user2.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user3.Id);
            singletonValueContainer.Users.Should().Contain(u => u == user4.Id);
        }
    }
}
