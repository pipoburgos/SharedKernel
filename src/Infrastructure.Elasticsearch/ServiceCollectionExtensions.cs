using Microsoft.Extensions.DependencyInjection;
using Nest;
using SharedKernel.Infrastructure.Elasticsearch.Client;

namespace SharedKernel.Infrastructure.Elasticsearch;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearch(this IServiceCollection services, Uri uri,
        string defaultIndex, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(typeof(ElasticClient),
            _ => new ElasticClient(new ConnectionSettings(uri).DefaultIndex(defaultIndex)), serviceLifetime));

        services.Add(new ServiceDescriptor(typeof(ElasticsearchClient),
            s => new ElasticsearchClient(s.GetRequiredService<ElasticClient>(),
                defaultIndex), serviceLifetime));

        services
            .AddHealthChecks()
            .AddElasticsearch(uri.ToString(), "Elasticsearch", tags: new[] { "DB", "Elasticsearch" });

        return services;
    }
}
