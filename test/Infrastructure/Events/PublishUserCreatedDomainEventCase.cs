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
            var user = UserMother.Create();

            await eventBus.Publish(user.PullDomainEvents(), CancellationToken.None).ConfigureAwait(false);

            await Task.Delay(millisecondsDelay, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(user.Id, singletonValueContainer.UserId);
            Assert.Equal(2, singletonValueContainer.Total);
        }
    }
}
