using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Docker;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.RabbitMq
{
    [Collection("DockerHook")]
    public class RabbitMqEventBusShould : InfrastructureTestCase
    {
        public RabbitMqEventBusShould(DockerHook dockerHook)
        {
            dockerHook.Run();
        }

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
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromRabbitMq()
        {
            await Task.Delay(5000);
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>(),
                GetRequiredService<PublishUserCreatedDomainEvent>(), 2_500);
        }
    }
}