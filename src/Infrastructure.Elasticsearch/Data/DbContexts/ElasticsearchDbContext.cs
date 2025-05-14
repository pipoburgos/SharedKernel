using Elastic.Clients.Elasticsearch;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Extensions;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

/// <summary> . </summary>
public abstract class ElasticsearchDbContext : DbContextAsync
{
    private readonly ElasticsearchClient _client;

    /// <summary> . </summary>
    protected ElasticsearchDbContext(ElasticsearchClient client, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(auditableService, classValidatorService)
    {
        _client = client;
    }

    /// <summary> . </summary>
    public string GetIndex<TAggregateRoot>() => typeof(TAggregateRoot).Name.ToKebabCase();

    /// <summary> . </summary>
    protected override void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        AddMethodAsync<TAggregateRoot, TId>(aggregateRoot, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary> . </summary>
    protected override void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        UpdateMethodAsync<TAggregateRoot, TId>(aggregateRoot, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary> . </summary>
    protected override void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        DeleteMethodAsync<TAggregateRoot, TId>(aggregateRoot, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary> . </summary>
    protected override async Task AddMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        await CreateIndexIfNotExistsAsync<TAggregateRoot>(cancellationToken);

        var response = await _client.IndexAsync(aggregateRoot, idx => idx
            .Index(GetIndex<TAggregateRoot>())
            .Id(aggregateRoot.Id.ToString()!), cancellationToken);

        response.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override async Task UpdateMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var response = await _client.IndexAsync(aggregateRoot, idx => idx
            .Index(GetIndex<TAggregateRoot>())
            .Id(aggregateRoot.Id.ToString()!), cancellationToken);

        response.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override async Task DeleteMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var response = await _client.DeleteAsync(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!,
            cancellationToken: cancellationToken);

        response.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    {
        return GetByIdAsync<TAggregateRoot, TId>(id, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary> . </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id,
        CancellationToken cancellationToken) where TAggregateRoot : class
    {
        var index = GetIndex<TAggregateRoot>();
        var existsIndex = await _client.Indices.ExistsAsync(index, cancellationToken);

        if (!existsIndex.Exists)
            return default;

        var searchResponse = await _client.GetAsync<TAggregateRoot>(index, id.ToString()!, cancellationToken);

        if (!searchResponse.IsValidResponse || !searchResponse.Found)
            return default;

        var aggregateRoot = searchResponse.Source;

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    private async Task CreateIndexIfNotExistsAsync<TAggregateRoot>(CancellationToken cancellationToken)
    {
        var exists = await _client.Indices.ExistsAsync(GetIndex<TAggregateRoot>(), cancellationToken);

        if (exists.Exists)
            return;

        var created = await _client.Indices.CreateAsync(GetIndex<TAggregateRoot>(),
            c => c.Settings(s => s.NumberOfReplicas(0)), cancellationToken);

        created.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    public async Task DeleteIndexAsync<TAggregateRoot>(CancellationToken cancellationToken)
    {
        var exists = await _client.Indices.ExistsAsync(GetIndex<TAggregateRoot>(), cancellationToken);

        if (!exists.Exists)
            return;

        var deleted = await _client.Indices.DeleteAsync(GetIndex<TAggregateRoot>(), cancellationToken);

        deleted.ThrowOriginalExceptionIfIsNotValid();
    }
}