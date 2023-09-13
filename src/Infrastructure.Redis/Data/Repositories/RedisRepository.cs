using SharedKernel.Domain.Aggregates;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Redis.Data.DbContexts;

namespace SharedKernel.Infrastructure.Redis.Data.Repositories;

/// <summary>  </summary>
public abstract class RedisRepository<TAggregateRoot, TId> : RepositoryAsync<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected RedisRepository(RedisDbContext redisDbContext) : base(redisDbContext)
    {
    }
}
