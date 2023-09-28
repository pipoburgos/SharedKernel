using Elastic.Clients.Elasticsearch;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using ConnectionConfiguration = Elasticsearch.Net.ConnectionConfiguration;

namespace SharedKernel.Infrastructure.Elasticsearch;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddElasticsearchHealthChecks(this IServiceCollection services, Uri uri,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var settings = new ElasticsearchClientSettings(uri);

        var connectionConfiguration = new ConnectionConfiguration(new SingleNodeConnectionPool(uri))
            //.DisableAutomaticProxyDetection()
            .EnableApiVersioningHeader()
            //.EnableHttpCompression()
            //.DisableDirectStreaming()
            //.PrettyJson()
            .RequestTimeout(TimeSpan.FromSeconds(30));

        //var connectionConfiguration = new ConnectionSettings(new SingleNodeConnectionPool(uri));

        services.Add(new ServiceDescriptor(typeof(ElasticsearchClient),
            _ => new ElasticsearchClient(settings), serviceLifetime));

        services.Add(new ServiceDescriptor(typeof(ElasticLowLevelClient),
            _ => new ElasticLowLevelClient(connectionConfiguration), serviceLifetime));

        services
            .AddHealthChecks()
            .AddElasticsearch(uri.ToString(), "Elasticsearch", tags: new[] { "DB", "Elasticsearch" });

        return services;
    }
}
