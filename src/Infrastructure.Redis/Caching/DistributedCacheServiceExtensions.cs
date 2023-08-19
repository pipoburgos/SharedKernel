using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Serializers;

namespace SharedKernel.Infrastructure.Redis.Caching;

/// <summary>  </summary>
public static class DistributedCacheServiceExtensions
{
    /// <summary>  </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRedisHealthChecks(configuration, "Redis Cache", "DistrubutedCache")
            .AddStackExchangeRedisCache(_ => { })
            .AddTransient<IBinarySerializer, BinarySerializer>()
            .AddTransient<ICacheHelper, DistributedCacheHelper>();
    }
}

