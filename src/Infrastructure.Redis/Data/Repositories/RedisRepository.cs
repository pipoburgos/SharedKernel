using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Redis.Data.Repositories;

/// <summary>  </summary>
public abstract class RedisRepository<TAggregateRoot, TId> : IReadRepository<TAggregateRoot>,
    IUpdateRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
{
    /// <summary>  </summary>
    protected readonly IDistributedCache DistributedCache;

    /// <summary>  </summary>
    protected readonly IBinarySerializer BinarySerializer;

    /// <summary>  </summary>
    protected readonly string AggregateName;

    /// <summary>  </summary>
    protected RedisRepository(
        IDistributedCache distributedCache,
        IBinarySerializer binarySerializer)
    {
        AggregateName = typeof(TAggregateRoot).Name;
        DistributedCache = distributedCache;
        BinarySerializer = binarySerializer;
    }

    /// <summary>  </summary>
    public TAggregateRoot? GetById<TId>(TId key)
    {
        var bytes = DistributedCache.Get(AggregateName + key);

        if (bytes == default || bytes.Length == 0)
            return default!;

        return BinarySerializer.Deserialize<TAggregateRoot>(bytes);
    }

    /// <summary>  </summary>
    public bool Any() => throw new NotImplementedException();

    /// <summary>  </summary>
    public bool NotAny() => throw new NotImplementedException();

    /// <summary>  </summary>
    public bool Any<TId>(TId key)
    {
        var bytes = DistributedCache.Get(AggregateName + key);

        return bytes != default && bytes.Length > 0;
    }

    /// <summary>  </summary>
    public bool NotAny<TId>(TId key)
    {
        var bytes = DistributedCache.Get(AggregateName + key);

        return bytes == default || bytes.Length == 0;
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot aggregate)
    {
        DistributedCache.Set(AggregateName + aggregate.Id, BinarySerializer.Serialize(aggregate));
    }

    /// <summary>  </summary>
    public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }
}
