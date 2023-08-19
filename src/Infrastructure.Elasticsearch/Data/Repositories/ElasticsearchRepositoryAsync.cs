using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Elasticsearch.Client;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepositoryAsync<TAggregateRoot> : ElasticsearchRepository<TAggregateRoot>,
    IPersistRepositoryAsync where TAggregateRoot : class, IAggregateRoot
{
    /// <summary>  </summary>
    /// <param name="client"></param>
    protected ElasticsearchRepositoryAsync(ElasticsearchClient client) : base(client)
    {
    }

    /// <summary>  </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    /// <summary>  </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
