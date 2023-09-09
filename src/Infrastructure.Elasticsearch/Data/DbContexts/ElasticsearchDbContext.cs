using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

/// <summary>  </summary>
public abstract class ElasticsearchDbContext : DbContext
{
    /// <summary>  </summary>
    protected readonly ElasticLowLevelClient Client;

    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected ElasticsearchDbContext(ElasticLowLevelClient client, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(auditableService,
        classValidatorService)
    {
        Client = client;
        JsonSerializer = jsonSerializer;
    }

    /// <summary>  </summary>
    protected string GetIndex<T>() => typeof(T).Name.ToLower();

    /// <summary>  </summary>
    protected override void AddMethod<T, TId>(T aggregateRoot)
    {
        // Realiza una solicitud HEAD al índice
        var exists = Client.Indices.Exists<StringResponse>(GetIndex<T>());

        if (!exists.Success)
            return;

        if (exists.HttpStatusCode != 404)
            return;

        var created = Client.Indices.Create<StringResponse>(GetIndex<T>(),
            PostData.Serializable(new { settings = new { number_of_replicas = 2 } }));

        if (!created.Success)
            throw created.OriginalException;

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
}
