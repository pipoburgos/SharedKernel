#pragma warning disable CS0618 // Type or member is obsolete
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

/// <summary> . </summary>
public abstract class ElasticsearchDbContext : DbContextAsync
{
    /// <summary> . </summary>
    public IJsonSerializer JsonSerializer { get; }

    /// <summary> . </summary>
    public readonly ElasticsearchClient Client;

    private readonly ElasticLowLevelClient _lowLevelClient;

    /// <summary> . </summary>
    protected ElasticsearchDbContext(ElasticsearchClient client, ElasticLowLevelClient lowLevelClient,
        IJsonSerializer jsonSerializer, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(auditableService, classValidatorService)
    {
        JsonSerializer = jsonSerializer;
        Client = client;
        _lowLevelClient = lowLevelClient;
    }

    /// <summary> . </summary>
    public string GetIndex<TAggregateRoot>() => typeof(TAggregateRoot).Name.ToLower();

    /// <summary> . </summary>
    protected override void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        CreateIndexIfNotExists<TAggregateRoot>();

        var added = Client.Create(aggregateRoot, GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!);
        added.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        var updated = Client.Index(aggregateRoot, index: GetIndex<TAggregateRoot>());
        updated.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        var deleted = Client.Delete<TAggregateRoot>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!);
        deleted.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override async Task AddMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        await CreateIndexIfNotExistsAsync<TAggregateRoot>(cancellationToken);

        var added = await Client
            .CreateAsync(aggregateRoot, GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!, cancellationToken);
        added.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override async Task UpdateMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var updated = await Client.IndexAsync(aggregateRoot, index: GetIndex<TAggregateRoot>(), cancellationToken);
        updated.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    protected override async Task DeleteMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var deleted = await Client
            .DeleteAsync<TAggregateRoot>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!, cancellationToken);
        deleted.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary> . </summary>
    public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    {
        var index = GetIndex<TAggregateRoot>();
        var existsIndex = Client.Indices.Exists(index);

        if (!existsIndex.Exists)
            return default;

        var existsDoc = Client.Exists<TAggregateRoot>(index, id.ToString()!);

        if (!existsDoc.Exists)
            return default;

        var searchResponse = _lowLevelClient.Get<StringResponse>(index, id.ToString()!);

        if (searchResponse.HttpStatusCode == 404)
            return default;

        if (searchResponse.HttpStatusCode != 200)
            throw searchResponse.OriginalException;

        return Deserialize<TAggregateRoot>(searchResponse);
    }

    /// <summary> . </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id,
        CancellationToken cancellationToken) where TAggregateRoot : class
    {
        var index = GetIndex<TAggregateRoot>();
        var existsIndex = await Client.Indices.ExistsAsync(index, cancellationToken);

        if (!existsIndex.Exists)
            return default;

        var existsDoc = await Client.ExistsAsync<TAggregateRoot>(index, id.ToString()!, cancellationToken);

        if (!existsDoc.Exists)
            return default;

        var searchResponse = await _lowLevelClient
            .GetAsync<StringResponse>(index, id.ToString()!, ctx: cancellationToken);

        if (searchResponse.HttpStatusCode == 404)
            return default;

        if (searchResponse.HttpStatusCode != 200)
            throw searchResponse.OriginalException;

        return Deserialize<TAggregateRoot>(searchResponse);
    }

    private TAggregateRoot? Deserialize<TAggregateRoot>(StringResponse searchResponse) where TAggregateRoot : class
    {
        var document = JsonSerializer
            .Deserialize<Dictionary<string, object>>(searchResponse.Body);

        var jsonAggregate = document["_source"].ToString();

        var aggregateRoot = string.IsNullOrWhiteSpace(jsonAggregate)
            ? default
            : JsonSerializer.Deserialize<TAggregateRoot?>(jsonAggregate);

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    private void CreateIndexIfNotExists<TAggregateRoot>()
    {
        var exists = Client.Indices.Exists(GetIndex<TAggregateRoot>());

        if (exists.Exists)
            return;

        var created = Client.Indices.Create(GetIndex<TAggregateRoot>(),
            x => x.Settings(new Elastic.Clients.Elasticsearch.IndexManagement.IndexSettings { NumberOfReplicas = 0 }));

        created.ThrowOriginalExceptionIfIsNotValid();
    }

    private async Task CreateIndexIfNotExistsAsync<TAggregateRoot>(CancellationToken cancellationToken)
    {
        var exists = await Client.Indices.ExistsAsync(GetIndex<TAggregateRoot>(), cancellationToken);

        if (!exists.Exists)
            return;

        var created = await Client.Indices.CreateAsync(GetIndex<TAggregateRoot>(),
            x => x.Settings(new Elastic.Clients.Elasticsearch.IndexManagement.IndexSettings { NumberOfReplicas = 0 }), cancellationToken);

        created.ThrowOriginalExceptionIfIsNotValid();
    }
}

#pragma warning restore CS0618 // Type or member is obsolete