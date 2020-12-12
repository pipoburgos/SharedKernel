using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.MassTransit
{
    public class MassTransitEventBusShould : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Events/MassTransit/appsettings.rabbitMq.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddMassTransitEventBus(Configuration, typeof(SetCountWhenUserCreatedSubscriber).Assembly)
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