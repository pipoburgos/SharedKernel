using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Redis.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.Redis;

public class RedisApp : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "System/Threading/Redis/appsettings.redis.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddRedisMutex(Configuration);
    }
}
