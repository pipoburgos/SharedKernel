using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Redis.Events;

namespace SharedKernel.Integration.Tests.Events.Redis;


public class RedisEventBusTests : EventBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Events/Redis/appsettings.redis.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddRedisEventBus(Configuration);
    }

    [Fact]
    public async Task PublishDomainEventFromRedis()
    {
        await PublishDomainEvent();
    }
}
