using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepository<TAggregateRoot, TId> : SaveRepository,
    IRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot, IEntity<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected string Index => typeof(TAggregateRoot).Name.ToLower();

    /// <summary>  </summary>
    protected readonly ElasticLowLevelClient Client;

    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected ElasticsearchRepository(UnitOfWork unitOfWork, ElasticLowLevelClient client, IJsonSerializer jsonSerializer)
        : base(unitOfWork)
    {
        Client = client;
        JsonSerializer = jsonSerializer;
    }

    /// <summary>  </summary>
    public void Add(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.AddOperation(() =>
        {
            Client.Index<StringResponse>(Index, aggregateRoot.Id.ToString(),
                JsonSerializer.Serialize(aggregateRoot));

            //if (!result.Success)
            //    throw new Exception(result.Body);
        }, aggregateRoot);
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

        return string.IsNullOrWhiteSpace(jsonAggregate)
            ? default
            : JsonSerializer.Deserialize<TAggregateRoot?>(jsonAggregate);
    }

    /// <summary>  </summary>
    public bool Any(TId id)
    {
        throw new NotImplementedException();
    }

    /// <summary>  </summary>
    public bool NotAny(TId id)
    {
        throw new NotImplementedException();
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.UpdateOperation(() =>
        {
            var result = Client.Index<StringResponse>(Index, aggregateRoot.Id.ToString(),
                JsonSerializer.Serialize(aggregateRoot));

            if (!result.Success)
                throw new Exception(result.Body);
        }, aggregateRoot);
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
        throw new NotImplementedException();
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        throw new NotImplementedException();
    }
}
