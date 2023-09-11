using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
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
    public string GetIndex<T>() => typeof(T).Name.ToLower();

    /// <summary>  </summary>
    protected override void AddMethod<T, TId>(T aggregateRoot)
    {
        var exists = Client.Indices.Exists<StringResponse>(GetIndex<T>());

        //if (!exists.Success)
        //    return;

        if (exists.HttpStatusCode == 404)
        {
            var created = Client.Indices.Create<StringResponse>(GetIndex<T>(),
                JsonSerializer.Serialize(new { settings = new { number_of_shards = 2, number_of_replicas = 2 } }));

            if (!created.Success)
                throw created.OriginalException;
        }

        var added = Client.Index<StringResponse>(GetIndex<T>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot));

        if (!added.Success)
            throw added.OriginalException;
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<T, TId>(T aggregateRoot)
    {
        var updated = Client.Index<StringResponse>(GetIndex<T>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot));

        if (!updated.Success)
            throw updated.OriginalException;
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<T, TId>(T aggregateRoot)
    {
        var deleted = Client.Delete<StringResponse>(GetIndex<T>(), aggregateRoot.Id.ToString());

        if (!deleted.Success)
            throw deleted.OriginalException;
    }

    /// <summary>  </summary>
    protected override async Task AddMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        var exists = await Client.Indices.ExistsAsync<StringResponse>(GetIndex<T>(), ctx: cancellationToken);

        //if (!exists.Success)
        //    return;

        if (exists.HttpStatusCode == 404)
        {
            var created = await Client.Indices.CreateAsync<StringResponse>(GetIndex<T>(),
                JsonSerializer.Serialize(new { settings = new { number_of_shards = 2, number_of_replicas = 2 } }),
                ctx: cancellationToken);

            if (!created.Success)
                throw created.OriginalException;
        }

        var added = await Client.IndexAsync<StringResponse>(GetIndex<T>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot), ctx: cancellationToken);

        if (!added.Success)
            throw added.OriginalException;
    }

    /// <summary>  </summary>
    protected override async Task UpdateMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        var updated = await Client.IndexAsync<StringResponse>(GetIndex<T>(), aggregateRoot.Id.ToString(),
            JsonSerializer.Serialize(aggregateRoot), ctx: cancellationToken);

        if (!updated.Success)
            throw updated.OriginalException;
    }

    /// <summary>  </summary>
    protected override async Task DeleteMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        var deleted =
            await Client.DeleteAsync<StringResponse>(GetIndex<T>(), aggregateRoot.Id.ToString(),
                ctx: cancellationToken);

        if (!deleted.Success)
            throw deleted.OriginalException;
    }
}
