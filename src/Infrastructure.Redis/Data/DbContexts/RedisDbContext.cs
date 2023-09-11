using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.Redis.Data.DbContexts;

/// <summary>  </summary>
public abstract class RedisDbContext : DbContextAsync
{
    /// <summary>  </summary>
    public IDistributedCache DistributedCache { get; }

    /// <summary>  </summary>
    public IJsonSerializer JsonSerializer { get; }

    /// <summary>  </summary>
    protected RedisDbContext(IDistributedCache distributedCache, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(auditableService,
        classValidatorService)
    {
        DistributedCache = distributedCache;
        JsonSerializer = jsonSerializer;
    }

    /// <summary>  </summary>
    protected string Get<T, TId>(TId id) => $"{typeof(T).Name}.{id}";

    /// <summary>  </summary>
    protected override void AddMethod<T, TId>(T aggregateRoot)
    {
        DistributedCache.SetString(Get<T, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<T, TId>(T aggregateRoot)
    {
        DistributedCache.SetString(Get<T, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<T, TId>(T aggregateRoot)
    {
        DistributedCache.Remove(Get<T, TId>(aggregateRoot.Id));
    }

    /// <summary>  </summary>
    protected override Task AddMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        return DistributedCache.SetStringAsync(Get<T, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot),
            token: cancellationToken);
    }

    /// <summary>  </summary>
    protected override Task UpdateMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        return DistributedCache.SetStringAsync(Get<T, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot),
            token: cancellationToken);
    }

    /// <summary>  </summary>
    protected override Task DeleteMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
    {
        return DistributedCache.RemoveAsync(Get<T, TId>(aggregateRoot.Id), cancellationToken);
    }
}
