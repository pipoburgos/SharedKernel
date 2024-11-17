using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.Redis.Data.DbContexts;

namespace SharedKernel.Infrastructure.Redis.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelRedisDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : RedisDbContext
    {
        return services
            .AddSharedKernelDbContext<TDbContext>(serviceLifetime)
            .AddRedisHealthChecks(configuration, "Redis UnitOfWork", "Redis UnitOfWork");
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelRedisUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : RedisDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelRedisDbContext<TDbContext>(configuration, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
