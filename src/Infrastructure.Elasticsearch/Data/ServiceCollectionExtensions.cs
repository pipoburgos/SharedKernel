using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Infrastructure.Elasticsearch.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddElasticsearchDbContext<TDbContext>(this IServiceCollection services, Uri uri,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : ElasticsearchDbContext
    {
        return services
            .AddDbContext<TDbContext>(serviceLifetime)
            .AddElasticsearchHealthChecks(uri, serviceLifetime);
    }

    /// <summary> . </summary>
    public static IServiceCollection AddElasticsearchUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        Uri uri, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : ElasticsearchDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddElasticsearchDbContext<TDbContext>(uri, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
