using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.RabbitMq
{
    [Collection("DockerHook")]
    public class RabbitMqEventBusShould : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Events/RabbitMq/appsettings.rabbitMq.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddDomainEvents(typeof(UserCreated))
                .AddRabbitMqEventBus(Configuration)
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>()
                .AddHttpContextAccessor();
        }

        [Fact]
        public async Task PublishDomainEventFromRabbitMq()
        {
            await Task.Delay(5_000);
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(this);
        }
    }
}