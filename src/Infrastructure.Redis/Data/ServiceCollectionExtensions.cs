using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Redis.Caching;
using SharedKernel.Infrastructure.Redis.Data.DbContexts;

namespace SharedKernel.Infrastructure.Redis.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddRedisDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : RedisDbContext
    {
        services.Add(new ServiceDescriptor(typeof(TDbContext), typeof(TDbContext), serviceLifetime));
        return services
            .AddSharedKernel()
            .AddRedisDistributedCache(configuration)
            .AddRedisHealthChecks(configuration, "Redis UnitOfWork", "Redis UnitOfWork");
    }

    /// <summary>  </summary>
    public static IServiceCollection AddRedisUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : RedisDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddRedisDbContext<TDbContext>(configuration, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
