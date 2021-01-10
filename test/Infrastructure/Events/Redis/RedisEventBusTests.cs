using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using SharedKernel.Domain.Tests.Users;
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
                .AddDomainEvents(typeof(UserCreated))
                .AddDomainEventsSubscribers(typeof(SetCountWhenUserCreatedSubscriber))
                .AddDomainEventSubscribers()
                .AddSingleton<PublishUserCreatedDomainEvent>();
        }

        [Fact]
        public async Task PublishDomainEventFromRedis()
        {
            var user = await PublishUserCreatedDomainEventCase.PublishDomainEvent(GetRequiredService<IEventBus>());
            var singletonValueContainer = GetRequiredService<PublishUserCreatedDomainEvent>();
            Assert.Equal(user.Id, singletonValueContainer.UserId);
            Assert.True(singletonValueContainer.Total >= 2);
        }
    }
}