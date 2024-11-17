using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Infrastructure.Elasticsearch.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelElasticsearchDbContext<TDbContext>(this IServiceCollection services, Uri uri,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : ElasticsearchDbContext
    {
        return services
            .AddSharedKernelDbContext<TDbContext>(serviceLifetime)
            .AddSharedKernelElasticsearchHealthChecks(uri, serviceLifetime);
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelElasticsearchUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        Uri uri, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : ElasticsearchDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelElasticsearchDbContext<TDbContext>(uri, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
