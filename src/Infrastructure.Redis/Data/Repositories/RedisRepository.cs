using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Redis.Data.Repositories;

/// <summary>  </summary>
public abstract class RedisRepository<TAggregateRoot, TId> : SaveRepository, IRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected readonly IDistributedCache DistributedCache;

    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected RedisRepository(
        UnitOfWork unitOfWork,
        IDistributedCache distributedCache,
        IJsonSerializer jsonSerializer) : base(unitOfWork)
    {
        DistributedCache = distributedCache;
        JsonSerializer = jsonSerializer;
    }

    /// <summary>  </summary>
    protected string GetKey(TId id)
    {
        return $"{typeof(TAggregateRoot).Name}.{id}";
    }

    /// <summary>  </summary>
    public void Add(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.AddOperation(aggregateRoot,
            () => DistributedCache.SetString(GetKey(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot)));
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
        var bytes = DistributedCache.GetString(GetKey(id));

        if (bytes == default || bytes.Length == 0)
            return default!;

        var aggregateRoot = JsonSerializer.Deserialize<TAggregateRoot?>(bytes);

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
        UnitOfWork.UpdateOperation(aggregateRoot,
            () => DistributedCache.SetString(GetKey(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot)));
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
        UnitOfWork.RemoveOperation(aggregateRoot, () => DistributedCache.Remove(GetKey(aggregateRoot.Id)),
            () => DistributedCache.SetString(GetKey(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot)));
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Remove(aggregateRoot);
        }
    }
}
