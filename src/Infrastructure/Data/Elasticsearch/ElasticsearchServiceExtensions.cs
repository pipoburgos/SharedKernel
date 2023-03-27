using Microsoft.Extensions.DependencyInjection;
using Nest;
using SharedKernel.Infrastructure.Data.Elasticsearch.Client;
using System;

namespace SharedKernel.Infrastructure.Data.Elasticsearch
{
    /// <summary>  </summary>
    public static class ElasticsearchServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="uri"></param>
        /// <param name="defaultIndex"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
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
}
