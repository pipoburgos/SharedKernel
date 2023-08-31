using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace SharedKernel.Infrastructure.Elasticsearch;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchHealthChecks(this IServiceCollection services, Uri uri,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(typeof(ElasticLowLevelClient),
            _ => new ElasticLowLevelClient(new ConnectionSettings(uri)), serviceLifetime));

        services
            .AddHealthChecks()
            .AddElasticsearch(uri.ToString(), "Elasticsearch", tags: new[] { "DB", "Elasticsearch" });

        return services;
    }
}
