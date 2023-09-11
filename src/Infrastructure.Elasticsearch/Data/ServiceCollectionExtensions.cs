using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Infrastructure.Elasticsearch.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchDbContext<TDbContext>(this IServiceCollection services, Uri uri,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : ElasticsearchDbContext
    {
        services.Add(new ServiceDescriptor(typeof(TDbContext), typeof(TDbContext), serviceLifetime));
        return services
            .AddDbContext()
            .AddElasticsearchHealthChecks(uri, serviceLifetime);
    }

    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        Uri uri, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : ElasticsearchDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddElasticsearchDbContext<TDbContext>(uri, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
