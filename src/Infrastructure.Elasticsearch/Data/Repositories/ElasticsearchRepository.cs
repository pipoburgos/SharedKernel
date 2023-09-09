using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepository<TAggregateRoot, TId> : SaveRepository,
    IRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected string Index => typeof(TAggregateRoot).Name.ToLower();

    /// <summary>  </summary>
    protected readonly ElasticLowLevelClient Client;

    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected ElasticsearchRepository(UnitOfWork unitOfWork, ElasticLowLevelClient client,
        IJsonSerializer jsonSerializer)
        : base(unitOfWork)
    {
        Client = client;
        JsonSerializer = jsonSerializer;

        // Realiza una solicitud HEAD al índice
        var exists = client.Indices.Exists<StringResponse>(Index);

        if (!exists.Success)
            return;

        if (exists.HttpStatusCode != 404)
            return;

        var response = client.Indices.Create<StringResponse>(Index,
            PostData.Serializable(new { settings = new { number_of_replicas = 2 } }));

        if (!response.Success)
            throw response.OriginalException;
    }

    /// <summary>  </summary>
    public void Add(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.AddOperation(aggregateRoot, () =>
        {
            var response = Client.Index<StringResponse>(Index, aggregateRoot.Id.ToString(),
                JsonSerializer.Serialize(aggregateRoot));

            if (!response.Success)
                throw response.OriginalException;
        });
    }

    /// <summary>  </summary>
    public void AddRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Add(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public TAggregateRoot? GetById(TId id)
    {
        var searchResponse = Client.Get<StringResponse>(Index, id.ToString());

        if (searchResponse.HttpStatusCode != 200)
            return default;

        var document = JsonSerializer.Deserialize<Dictionary<string, object>>(searchResponse.Body);
        var jsonAggregate = document["_source"].ToString();

        var aggregateRoot = string.IsNullOrWhiteSpace(jsonAggregate)
            ? default
            : JsonSerializer.Deserialize<TAggregateRoot?>(jsonAggregate);

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a) ? default : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    public bool Any(TId id)
    {
        return GetById(id) != default;
    }

    /// <summary>  </summary>
    public bool NotAny(TId id)
    {
        return GetById(id) == default;
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.UpdateOperation(aggregateRoot, () =>
        {
            var response = Client.Index<StringResponse>(Index, aggregateRoot.Id.ToString(),
                JsonSerializer.Serialize(aggregateRoot));

            if (!response.Success)
                throw response.OriginalException;
        });
    }

    /// <summary>  </summary>
    public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public void Remove(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.RemoveOperation(aggregateRoot, () =>
            {
                var response = Client.Delete<StringResponse>(Index, aggregateRoot.Id.ToString());

                if (!response.Success)
                    throw response.OriginalException;
            }, () =>
            {
                var response = Client.Index<StringResponse>(Index, aggregateRoot.Id.ToString(),
                    JsonSerializer.Serialize(aggregateRoot));

                if (!response.Success)
                    throw response.OriginalException;
            }
        );
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }
}
