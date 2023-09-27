using System.Net.Http;
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
    public readonly ElasticLowLevelClient Client;

    /// <summary>  </summary>
    public readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected ElasticsearchDbContext(ElasticLowLevelClient client, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(auditableService,
        classValidatorService)
    {
        Client = client;
        JsonSerializer = jsonSerializer;
    }

    /// <summary>  </summary>
    public string GetIndex<TAggregateRoot>() => typeof(TAggregateRoot).Name.ToLower();

    /// <summary>  </summary>
    protected override void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        //var exists = Client.Indices.Exists<StringResponse>(GetIndex<TAggregateRoot>());

        //if (exists.HttpStatusCode == 404)
        //{
        //    var created = Client.Indices.Create<StringResponse>(GetIndex<TAggregateRoot>(),
        //        JsonSerializer.Serialize(new { settings = new { number_of_shards = 2, number_of_replicas = 2 } }));

        //    if (!created.Success)
        //        throw created.OriginalException;
        //}

        var added = Client.Index<StringResponse>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot));

        if (!added.Success)
            throw added.OriginalException;
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        var updated = Client.Index<StringResponse>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot));

        if (!updated.Success)
            throw updated.OriginalException;
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        var deleted = Client.Delete<StringResponse>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString());

        if (!deleted.Success)
            throw deleted.OriginalException;
    }

    /// <summary>  </summary>
    protected override async Task AddMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        //var exists = await Client.Indices
        //    .ExistsAsync<StringResponse>(GetIndex<TAggregateRoot>(), ctx: cancellationToken);

        //if (exists.HttpStatusCode == 404)
        //{
        //    var created = await Client.Indices.CreateAsync<StringResponse>(GetIndex<TAggregateRoot>(),
        //        JsonSerializer.Serialize(new { settings = new { number_of_shards = 2, number_of_replicas = 2 } }),
        //        ctx: cancellationToken);

        //    if (!created.Success)
        //        throw created.OriginalException;
        //}

        var added = await Client.IndexAsync<StringResponse>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot), ctx: cancellationToken);

        if (!added.Success)
            throw added.OriginalException;
    }

    /// <summary>  </summary>
    protected override async Task UpdateMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var updated = await Client.IndexAsync<StringResponse>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot), ctx: cancellationToken);

        if (!updated.Success)
            throw updated.OriginalException;
    }

    /// <summary>  </summary>
    protected override async Task DeleteMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        var deleted =
            await Client.DeleteAsync<StringResponse>(GetIndex<TAggregateRoot>(), aggregateRoot.Id.ToString(),
                ctx: cancellationToken);

        if (!deleted.Success)
            throw deleted.OriginalException;
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    {
        var exists =
            Client.Indices.Exists<StringResponse>(GetIndex<TAggregateRoot>());

        if (exists.HttpStatusCode == 404)
            return default;

        var searchResponse = Client.Get<StringResponse>(GetIndex<TAggregateRoot>(), id.ToString());

        if (searchResponse.HttpStatusCode == 404)
            return default;

        if (searchResponse.HttpStatusCode != 200)
            throw searchResponse.OriginalException;

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

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id,
        CancellationToken cancellationToken) where TAggregateRoot : class
    {
        var exists = await Client.Indices
            .ExistsAsync<StringResponse>(GetIndex<TAggregateRoot>(), ctx: cancellationToken);

        if (exists.HttpStatusCode == 404)
            return default;

        var searchResponse = await Client.GetAsync<StringResponse>(
            GetIndex<TAggregateRoot>(), id.ToString(), ctx: cancellationToken);

        if (searchResponse.HttpStatusCode == 404)
            return default;

        if (searchResponse.HttpStatusCode != 200)
        {
            var client = new HttpClient();

            var result =
                await client.GetAsync($"http://admin:password@127.0.0.1:22228/{GetIndex<TAggregateRoot>()}/_doc/{id}",
                    cancellationToken);

            var st = await result.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception(st);
            //throw searchResponse.OriginalException;
        }

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
}
