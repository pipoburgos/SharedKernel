using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.InMemory
{
    public class InMemoryEventBusTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Events/InMemory/appsettings.inMemory.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddInMemoryEventBus(Configuration)
                .AddDomainEvents(typeof(UserCreated))
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>()
                .AddHttpContextAccessor();
        }

        [Fact]
        public async Task PublishDomainEventFromMemory()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(this);
        }
    }
}
