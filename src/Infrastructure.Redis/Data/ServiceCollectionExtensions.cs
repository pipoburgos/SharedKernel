using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.UnitOfWorks;
using SharedKernel.Infrastructure.Redis.Caching;

namespace SharedKernel.Infrastructure.Redis.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddRedisUnitOfWork<TInterface, TClass>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TClass : UnitOfWork, TInterface
        where TInterface : IUnitOfWork
    {
        services.Add(new ServiceDescriptor(typeof(UnitOfWork), typeof(UnitOfWork), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services
            .AddRedisDistributedCache(configuration)
            .AddRedisHealthChecks(configuration, "Redis UnitOfWork", "Redis UnitOfWork");
    }

    /// <summary>  </summary>
    public static IServiceCollection AddRedisUnitOfWorkAsync<TInterface, TClass>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TClass : UnitOfWorkAsync, TInterface
        where TInterface : IUnitOfWorkAsync
    {
        services.Add(new ServiceDescriptor(typeof(UnitOfWorkAsync), typeof(UnitOfWorkAsync), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services
            .AddRedisUnitOfWork<TInterface, TClass>(configuration);
    }
}
