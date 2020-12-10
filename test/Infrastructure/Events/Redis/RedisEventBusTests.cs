using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.Redis
{
    public class RedisEventBusTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Events/Redis/appsettings.redis.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddRedisEventBus(Configuration)
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber).Assembly)
                .AddDomainEventSubscribersInformation()
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromRedis()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>(),
                GetRequiredService<PublishUserCreatedDomainEvent>(), 750);
        }
    }
}