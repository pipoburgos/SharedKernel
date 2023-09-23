using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Redis.Cqrs.Commands;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.Redis;

[Collection("DockerHook")]
public class RedisCommandBusShould : CommandBusCommonTestCase
{
    protected override string GetJsonFile()
    {
        return "Cqrs/Commands/Redis/appsettings.redis.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddRedisCommandBusAsync(Configuration);
    }

    [Fact]
    public async Task DispatchCommandAsyncFromRabbitMq()
    {
        await DispatchCommandAsync();
    }
}
