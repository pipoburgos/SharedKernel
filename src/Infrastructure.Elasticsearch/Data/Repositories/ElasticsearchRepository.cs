using Elasticsearch.Net;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepository<TAggregateRoot, TId> : RepositoryAsync<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    private readonly ElasticsearchDbContext _elasticsearchDbContext;

    /// <summary>  </summary>
    protected ElasticsearchRepository(ElasticsearchDbContext elasticsearchDbContext) : base(elasticsearchDbContext)
    {
        _elasticsearchDbContext = elasticsearchDbContext;
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById(TId id)
    {
        var searchResponse =
            _elasticsearchDbContext.Client.Get<StringResponse>(_elasticsearchDbContext.GetIndex<TAggregateRoot>(),
                id.ToString());

        if (searchResponse.HttpStatusCode != 200)
            throw searchResponse.OriginalException;

        var document = _elasticsearchDbContext.JsonSerializer
            .Deserialize<Dictionary<string, object>>(searchResponse.Body);

        var jsonAggregate = document["_source"].ToString();

        var aggregateRoot = string.IsNullOrWhiteSpace(jsonAggregate)
            ? default
            : _elasticsearchDbContext.JsonSerializer.Deserialize<TAggregateRoot?>(jsonAggregate);

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var searchResponse = await _elasticsearchDbContext.Client.GetAsync<StringResponse>(
            _elasticsearchDbContext.GetIndex<TAggregateRoot>(), id.ToString(), ctx: cancellationToken);

        if (searchResponse.HttpStatusCode != 200)
            throw searchResponse.OriginalException;

        var document = _elasticsearchDbContext.JsonSerializer
            .Deserialize<Dictionary<string, object>>(searchResponse.Body);

        var jsonAggregate = document["_source"].ToString();

        var aggregateRoot = string.IsNullOrWhiteSpace(jsonAggregate)
            ? default
            : _elasticsearchDbContext.JsonSerializer.Deserialize<TAggregateRoot?>(jsonAggregate);

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }
}
