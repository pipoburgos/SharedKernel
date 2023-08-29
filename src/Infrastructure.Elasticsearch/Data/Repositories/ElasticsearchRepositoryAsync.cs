using SharedKernel.Domain.Aggregates;
using SharedKernel.Infrastructure.Elasticsearch.Client;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepositoryAsync<TAggregateRoot> : ElasticsearchRepository<TAggregateRoot>
    where TAggregateRoot : class, IAggregateRoot
{
    /// <summary>  </summary>
    protected ElasticsearchRepositoryAsync(ElasticsearchClient client) : base(client)
    {
    }
}
