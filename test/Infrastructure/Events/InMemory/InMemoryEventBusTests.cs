using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
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
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>(),
                GetRequiredService<PublishUserCreatedDomainEvent>(), 1_500);
        }
    }
}
