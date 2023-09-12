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
        var exists =
            _elasticsearchDbContext.Client.Indices.Exists<StringResponse>(_elasticsearchDbContext
                .GetIndex<TAggregateRoot>());

        if (exists.HttpStatusCode == 404)
            return null;

        var searchResponse =
        _elasticsearchDbContext.Client.Get<StringResponse>(_elasticsearchDbContext.GetIndex<TAggregateRoot>(),
            id.ToString());

        if (searchResponse.HttpStatusCode != 200)
            return null;// throw searchResponse.OriginalException;

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
        var exists =
            await _elasticsearchDbContext.Client.Indices.ExistsAsync<StringResponse>(
                _elasticsearchDbContext.GetIndex<TAggregateRoot>(), ctx: cancellationToken);

        if (exists.HttpStatusCode == 404)
            return null;

        var searchResponse = await _elasticsearchDbContext.Client.GetAsync<StringResponse>(
            _elasticsearchDbContext.GetIndex<TAggregateRoot>(), id.ToString(), ctx: cancellationToken);

        if (searchResponse.HttpStatusCode != 200)
            return null;//throw searchResponse.OriginalException;

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
