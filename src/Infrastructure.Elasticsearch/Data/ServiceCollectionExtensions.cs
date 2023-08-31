using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Elasticsearch.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchUnitOfWork<TInterface, TClass>(this IServiceCollection services,
        Uri uri, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TClass : UnitOfWork, TInterface
        where TInterface : IUnitOfWork
    {
        services.Add(new ServiceDescriptor(typeof(UnitOfWork), typeof(UnitOfWork), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services
            .AddSharedKernel()
            .AddElasticsearchHealthChecks(uri, serviceLifetime);
    }

    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchUnitOfWorkAsync<TInterface, TClass>(
        this IServiceCollection services, Uri uri, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TClass : UnitOfWorkAsync, TInterface where TInterface : IUnitOfWorkAsync
    {
        services.Add(new ServiceDescriptor(typeof(UnitOfWorkAsync), typeof(UnitOfWorkAsync), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services.AddElasticsearchUnitOfWork<TInterface, TClass>(uri, serviceLifetime);
    }
}
