using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Serialization;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace SharedKernel.Infrastructure.Elasticsearch;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelElasticsearchHealthChecks(this IServiceCollection services, Uri uri,
        Action<JsonSerializerOptions>? configureOptions, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var pool = new SingleNodePool(uri);

        services.AddScoped(_ =>
        {
            return new ElasticsearchClientSettings(pool, (_, setti) =>
                    new DefaultSourceSerializer(setti, configureOptions))
                .DisableDirectStreaming()
                .EnableDebugMode();
        });

        services.AddScoped(serviceProvider => new ElasticsearchClient(serviceProvider.GetRequiredService<ElasticsearchClientSettings>()));

        services
            .AddHealthChecks()
            .AddElasticsearch(uri.ToString(), "Elasticsearch", tags: ["DB", "Elasticsearch"]);

        return services;
    }
}
