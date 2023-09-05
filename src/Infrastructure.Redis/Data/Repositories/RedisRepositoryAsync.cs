using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Redis.Data.Repositories;

/// <summary>  </summary>
public abstract class RedisRepositoryAsync<TAggregateRoot, TId> : RedisRepository<TAggregateRoot, TId>,
    IRepositoryAsync<TAggregateRoot, TId>,
    ISaveRepositoryAsync where TAggregateRoot : class, IAggregateRoot, IEntity<TId> where TId : notnull
{
    private readonly UnitOfWorkAsync _unitOfWorkAsync;

    /// <summary>  </summary>
    protected RedisRepositoryAsync(UnitOfWorkAsync unitOfWorkAsync, IDistributedCache distributedCache,
        IJsonSerializer jsonSerializer) : base(unitOfWorkAsync, distributedCache, jsonSerializer)
    {
        _unitOfWorkAsync = unitOfWorkAsync;
    }

    /// <summary>  </summary>
    public Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.AddOperationAsync(aggregateRoot,
            () => DistributedCache.SetStringAsync(GetKey(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot),
                token: cancellationToken));
    }

    /// <summary>  </summary>
    public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(a => AddAsync(a, cancellationToken)));
    }

    /// <summary>  </summary>
    public async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var bytes = await DistributedCache.GetStringAsync(GetKey(id), token: cancellationToken);

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
    public async Task<bool> AnyAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) != default;
    }

    /// <summary>  </summary>
    public async Task<bool> NotAnyAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) == default;
    }

    /// <summary>  </summary>
    public Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.UpdateOperationAsync(aggregateRoot,
            () => DistributedCache.SetStringAsync(GetKey(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot),
                token: cancellationToken));
    }

    /// <summary>  </summary>
    public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(a => UpdateAsync(a, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.RemoveOperationAsync(aggregateRoot,
            () => DistributedCache.RemoveAsync(GetKey(aggregateRoot.Id), cancellationToken),
            () => DistributedCache.SetStringAsync(GetKey(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot),
                token: cancellationToken));
    }

    /// <summary>  </summary>
    public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(a => RemoveAsync(a, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.SaveChangesResultAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.RollbackAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.RollbackResultAsync(cancellationToken);
    }
}
