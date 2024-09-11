using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.Redis.Caching;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddRedisHealthChecks(configuration, "Redis Cache", "DistributedCache");
    }
}

