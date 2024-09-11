using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Serializers;

namespace SharedKernel.Infrastructure.Redis;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    internal static IServiceCollection AddRedisHealthChecks(this IServiceCollection services, IConfiguration configuration,
        string name, params string[] tags)
    {
        var tagsList = tags.ToList();
        tagsList.Add("Redis");

        services
            .AddRedisOptions(configuration)
            .AddHealthChecks()
            .AddRedis(sp => sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value.Configuration!, name, tags: tagsList);

        return services;
    }

    /// <summary> . </summary>
    internal static IServiceCollection AddRedisOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddOptions()
            .Configure<RedisCacheOptions>(configuration.GetSection(nameof(RedisCacheOptions)))
            .AddStackExchangeRedisCache(_ => { })
            .AddTransient<IBinarySerializer, BinarySerializer>()
            .AddTransient<ICacheHelper, DistributedCacheHelper>();
    }
}
