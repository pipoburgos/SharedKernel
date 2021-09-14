using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Docker;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Events.Redis
{
    [Collection("DockerHook")]
    public class RedisEventBusTests : InfrastructureTestCase
    {
        public RedisEventBusTests(DockerHook dockerHook)
        {
            dockerHook.Run();
        }

        protected override string GetJsonFile()
        {
            return "Events/Redis/appsettings.redis.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddRedisEventBus(Configuration)
                .AddDomainEvents(typeof(UserCreated))
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromRedis()
        {
            await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>(),
                GetRequiredService<PublishUserCreatedDomainEvent>(), 2_500);
        }
    }
}