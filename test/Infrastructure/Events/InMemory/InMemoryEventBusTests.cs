using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using SharedKernel.Domain.Tests.Users;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.InMemory
{
    public class InMemoryEventBusTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddInMemoryEventBus()
                .AddDomainEvents(typeof(UserCreated))
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromMemory()
        {
            var user = await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>());
            var singletonValueContainer = GetRequiredService<PublishUserCreatedDomainEvent>();
            Assert.Equal(user.Id, singletonValueContainer.UserId);
            Assert.True(singletonValueContainer.Total >= 2);
        }
    }
}
