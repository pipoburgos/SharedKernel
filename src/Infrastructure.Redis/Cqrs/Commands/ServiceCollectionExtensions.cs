using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Cqrs.Commands;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddRedisCommandBusAsync(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddRedisHealthChecks(configuration, "Redis Async Command Bus", "Command Bus")
            .AddHostedService<RedisCommandsConsumer>()
            .AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value
                    .Configuration!))
            .AddTransient<ICommandBusAsync, RedisCommandBusAsync>();
    }
}