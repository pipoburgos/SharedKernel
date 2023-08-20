using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.Redis.System.Threading;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register Redis IMutexManager and IMutex factory. </summary>
    public static IServiceCollection AddRedisMutex(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRedisHealthChecks(configuration, "Redis Mutex", "Mutex")
            .AddTransient<IMutexManager, MutexManager>()
            .AddTransient<IMutexFactory, RedisMutexFactory>();
    }
}
