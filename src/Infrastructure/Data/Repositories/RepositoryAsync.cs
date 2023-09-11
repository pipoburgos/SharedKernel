using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Infrastructure.Data.DbContexts;

namespace SharedKernel.Infrastructure.Data.Repositories;

/// <summary>  </summary>
public abstract class RepositoryAsync<TAggregateRoot, TId> : Repository<TAggregateRoot, TId>,
    IRepositoryAsync<TAggregateRoot, TId>,
    ISaveRepositoryAsync where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    private readonly IDbContextAsync _dbContextAsync;

    /// <summary>  </summary>
    protected RepositoryAsync(IDbContextAsync dbContextAsync) : base(dbContextAsync)
    {
        _dbContextAsync = dbContextAsync;
    }

    /// <summary>  </summary>
    public Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return _dbContextAsync.AddAsync<TAggregateRoot, TId>(aggregateRoot, cancellationToken);
    }

    /// <summary>  </summary>
    public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(aggregateRoot => AddAsync(aggregateRoot, cancellationToken)));
    }

    /// <summary>  </summary>
    public abstract Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken);

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
    public async Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        var originalAggregate = await GetByIdAsync(aggregateRoot.Id, cancellationToken);
        await _dbContextAsync.UpdateAsync<TAggregateRoot, TId>(aggregateRoot, originalAggregate!, cancellationToken);
    }

    /// <summary>  </summary>
    public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(aggregateRoot => UpdateAsync(aggregateRoot, cancellationToken)));
    }

    /// <summary>  </summary>
    public async Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        var originalAggregate = await GetByIdAsync(aggregateRoot.Id, cancellationToken);
        await _dbContextAsync.RemoveAsync<TAggregateRoot, TId>(aggregateRoot, originalAggregate!, cancellationToken);
    }

    /// <summary>  </summary>
    public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(aggregateRoot => RemoveAsync(aggregateRoot, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContextAsync.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return _dbContextAsync.SaveChangesResultAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return _dbContextAsync.RollbackAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return _dbContextAsync.RollbackResultAsync(cancellationToken);
    }
}