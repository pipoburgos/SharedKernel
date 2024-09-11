using SharedKernel.Domain.Aggregates;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary> . </summary>
public abstract class ElasticsearchRepository<TAggregateRoot, TId> : RepositoryAsync<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary> . </summary>
    protected ElasticsearchRepository(ElasticsearchDbContext elasticsearchDbContext) : base(elasticsearchDbContext)
    {
    }
}
