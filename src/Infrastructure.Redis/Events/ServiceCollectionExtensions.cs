using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Events;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Events;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddRedisEventBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddRedisHealthChecks(configuration, "Redis Event Bus", "Event Bus")
            .AddHostedService<RedisDomainEventsConsumer>()
            .AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value
                    .Configuration!))
            .AddTransient<IEventBus, RedisEventBus>();
    }
}
