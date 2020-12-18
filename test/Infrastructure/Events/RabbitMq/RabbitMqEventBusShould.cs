using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.RabbitMq
{
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
                .AddRabbitMqEventBus(Configuration, new[] { typeof(UserCreated).Assembly })
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber).Assembly)
                .AddDomainEventSubscribersInformation()
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromRabbitMq()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>(),
                GetRequiredService<PublishUserCreatedDomainEvent>(), 500);
        }
    }
}