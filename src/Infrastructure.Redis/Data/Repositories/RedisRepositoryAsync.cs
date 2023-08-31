using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Redis.Data.Repositories;

/// <summary>  </summary>
public abstract class RedisRepositoryAsync<TAggregateRoot, TId> : RedisRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
{
    /// <summary>  </summary>
    protected RedisRepositoryAsync(UnitOfWorkAsync unitOfWorkAsync, IDistributedCache distributedCache,
        IJsonSerializer jsonSerializer) : base(unitOfWorkAsync, distributedCache, jsonSerializer)
    {
    }
}
