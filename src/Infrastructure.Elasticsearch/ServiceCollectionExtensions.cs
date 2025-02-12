using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.Elasticsearch;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelElasticsearchHealthChecks(this IServiceCollection services, Uri uri,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var pool = new SingleNodePool(uri);

        var settings = new ElasticsearchClientSettings(pool, (_, _) => new CustomElasticsearchSerializer()).DisableDirectStreaming();

        services.Add(new ServiceDescriptor(typeof(ElasticsearchClient),
            _ => new ElasticsearchClient(settings), serviceLifetime));

        services
            .AddHealthChecks()
            .AddElasticsearch(uri.ToString(), "Elasticsearch", tags: ["DB", "Elasticsearch"]);

        return services;
    }
}
