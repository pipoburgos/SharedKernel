using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Redis.Data.DbContexts;

namespace SharedKernel.Infrastructure.Redis.Data.Repositories;

/// <summary>  </summary>
public abstract class RedisRepository<TAggregateRoot, TId> : RepositoryAsync<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    private readonly RedisDbContext _redisDbContext;

    /// <summary>  </summary>
    protected RedisRepository(RedisDbContext redisDbContext) : base(redisDbContext)
    {
        _redisDbContext = redisDbContext;
    }

    /// <summary>  </summary>
    protected string GetKey(TId id)
    {
        return $"{typeof(TAggregateRoot).Name}.{id}";
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById(TId id)
    {
        var bytes = _redisDbContext.DistributedCache.GetString(GetKey(id));

        if (bytes == default || bytes.Length == 0)
            return default!;

        var aggregateRoot = _redisDbContext.JsonSerializer.Deserialize<TAggregateRoot?>(bytes);

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var bytes = await _redisDbContext.DistributedCache.GetStringAsync(GetKey(id), cancellationToken);

        if (bytes == default || bytes.Length == 0)
            return default!;

        var aggregateRoot = _redisDbContext.JsonSerializer.Deserialize<TAggregateRoot?>(bytes);

        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }
}
