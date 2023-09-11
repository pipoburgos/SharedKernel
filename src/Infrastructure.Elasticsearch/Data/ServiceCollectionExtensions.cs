using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Infrastructure.Elasticsearch.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchUnitOfWork<TInterface, TClass>(this IServiceCollection services,
        Uri uri, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TClass : ElasticsearchDbContext, TInterface
        where TInterface : IDbContextAsync
    {
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services
            .AddDbContext()
            .AddElasticsearchHealthChecks(uri, serviceLifetime);
    }
}
