using SharedKernel.Domain.Events;
using SharedKernel.Domain.Tests.Users;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Integration.Tests.Events
{
    internal static class PublishUserCreatedDomainEventCase
    {
        internal static async Task<User> PublishDomainEvent(IEventBus eventBus)
        {
            var user = UserMother.Create();

            await eventBus.Publish(user.PullDomainEvents(), CancellationToken.None).ConfigureAwait(false);

            await Task.Delay(2_500, CancellationToken.None).ConfigureAwait(false);

            return user;
        }
    }
}
