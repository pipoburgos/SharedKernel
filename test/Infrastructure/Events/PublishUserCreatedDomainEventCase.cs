using SharedKernel.Domain.Events;
using SharedKernel.Domain.Tests.Users;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
            await eventBus.Publish(domainEvents, CancellationToken.None).ConfigureAwait(false);

            await Task.Delay(millisecondsDelay, CancellationToken.None).ConfigureAwait(false);

            Assert.Contains(singletonValueContainer.Users, u => u == user1.Id);
            Assert.Contains(singletonValueContainer.Users, u => u == user2.Id);
            Assert.Contains(singletonValueContainer.Users, u => u == user3.Id);
            Assert.Contains(singletonValueContainer.Users, u => u == user4.Id);
            Assert.True(singletonValueContainer.Total == 4);
        }
    }
}
