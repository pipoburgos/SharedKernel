using Elastic.Clients.Elasticsearch;
using Elastic.Transport.Products.Elasticsearch;
using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

/// <summary>  </summary>
public abstract class ElasticsearchDbContext : DbContextAsync
{
    /// <summary>  </summary>
    public IJsonSerializer JsonSerializer { get; }

    /// <summary>  </summary>
    public readonly ElasticsearchClient Client;

    private readonly ElasticLowLevelClient _lowLevelClient;

    /// <summary>  </summary>
    protected ElasticsearchDbContext(ElasticsearchClient client, ElasticLowLevelClient lowLevelClient,
        IJsonSerializer jsonSerializer, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(auditableService, classValidatorService)
    {
        JsonSerializer = jsonSerializer;
        Client = client;
        _lowLevelClient = lowLevelClient;
    }

    /// <summary>  </summary>
    public string GetIndex<TAggregateRoot>() => typeof(TAggregateRoot).Name.ToLower();

    /// <summary>  </summary>
    protected override void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        CreateIndexIfNotExists<TAggregateRoot>();

        var added = Client.Index(aggregateRoot, GetIndex<TAggregateRoot>());

        added.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        var updated = Client.Update(aggregateRoot, aggregateRoot, GetIndex<TAggregateRoot>(),
            aggregateRoot.Id.ToString()!);

        updated.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        var deleted = Client.Delete(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!);

        deleted.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary>  </summary>
    protected override async Task AddMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        await CreateIndexIfNotExistsAsync<TAggregateRoot>(cancellationToken);

        var added = await Client.IndexAsync(aggregateRoot, GetIndex<TAggregateRoot>(), cancellationToken);

        added.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary>  </summary>
    protected override async Task UpdateMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var updated = await Client.UpdateAsync(aggregateRoot, aggregateRoot, GetIndex<TAggregateRoot>(),
            aggregateRoot.Id.ToString()!, cancellationToken);

        updated.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary>  </summary>
    protected override async Task DeleteMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var deleted = await Client.DeleteAsync(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString()!, cancellationToken);

        deleted.ThrowOriginalExceptionIfIsNotValid();
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    {
        var existsIndex = Client.Indices.Exists(GetIndex<TAggregateRoot>());

        if (!existsIndex.Exists)
            return default;

        var existsDoc = Client.Exists(GetIndex<TAggregateRoot>(), id.ToString()!);

        if (!existsDoc.Exists)
            return default;

        var searchResponse = _lowLevelClient.Get<StringResponse>(GetIndex<TAggregateRoot>(), id.ToString()!);

        return searchResponse.ThrowOriginalExceptionIfIsNotValid() ? default : Deserialize<TAggregateRoot>(searchResponse);
    }

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id,
        CancellationToken cancellationToken) where TAggregateRoot : class
    {
        var existsIndex = await Client.Indices.ExistsAsync(GetIndex<TAggregateRoot>(), cancellationToken);

        if (!existsIndex.Exists)
            return default;

        var existsDoc = await Client.ExistsAsync(GetIndex<TAggregateRoot>(), id.ToString()!, cancellationToken);

        if (!existsDoc.Exists)
            return default;

        var searchResponse = await _lowLevelClient
            .GetAsync<StringResponse>(GetIndex<TAggregateRoot>(), id.ToString()!, ctx: cancellationToken);

        return searchResponse.ThrowOriginalExceptionIfIsNotValid() ? default : Deserialize<TAggregateRoot>(searchResponse);
    }

    ///// <summary>  </summary>
    //public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    //{
    //    var existsIndex = Client.Indices.Exists(GetIndex<TAggregateRoot>());

    //    if (!existsIndex.Exists)
    //        return default;

    //    var existsDoc = Client.Exists(GetIndex<TAggregateRoot>(), id.ToString()!);

    //    if (!existsDoc.Exists)
    //        return default;

    //    var search = Client.Get<TAggregateRoot>(GetIndex<TAggregateRoot>(), id.ToString()!);

    //    search.ThrowOriginalExceptionIfIsNotValid();

    //    var aggregateRoot = search.Source;

    //    if (aggregateRoot == null)
    //        return default;

    //    if (aggregateRoot is IEntityAuditableLogicalRemove a)
    //    {
    //        return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
    //            ? default
    //            : aggregateRoot;
    //    }

    //    return aggregateRoot;
    //}

    ///// <summary>  </summary>
    //public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id,
    //    CancellationToken cancellationToken) where TAggregateRoot : class
    //{
    //    var existsIndex = await Client.Indices.ExistsAsync(GetIndex<TAggregateRoot>(), cancellationToken);

    //    if (!existsIndex.Exists)
    //        return default;

    //    var existsDoc = await Client.ExistsAsync(GetIndex<TAggregateRoot>(), id.ToString()!, cancellationToken);

    //    if (!existsDoc.Exists)
    //        return default;

    //    var search = Client.Get<TAggregateRoot>(GetIndex<TAggregateRoot>(), id.ToString()!);

    //    search.ThrowOriginalExceptionIfIsNotValid();

    //    var aggregateRoot = search.Source;

    //    if (aggregateRoot == null)
    //        return default;

    //    if (aggregateRoot is IEntityAuditableLogicalRemove a)
    //    {
    //        return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
    //            ? default
    //            : aggregateRoot;
    //    }

    //    return aggregateRoot;
    //}

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

/// <summary>  </summary>
public static class Response
{
    /// <summary>  </summary>
    public static void ThrowOriginalExceptionIfIsNotValid(this ElasticsearchResponse response)
    {
        if (response.IsValidResponse)
            return;

        if (!response.TryGetOriginalException(out var ex))
            return;

        if (ex != null)
            throw ex;
    }

    /// <summary>  </summary>
    public static bool ThrowOriginalExceptionIfIsNotValid(this ElasticsearchResponseBase response)
    {
        if (response.HttpStatusCode == 404)
            return true;

        if (response.HttpStatusCode != 200)
            throw response.OriginalException;

        return false;
    }
}