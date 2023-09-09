using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary>  </summary>
public interface IDbContextAsync : IUnitOfWorkAsync
{
    /// <summary>  </summary>
    Task AddAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    Task UpdateAsync<T, TId>(T aggregateRoot, T originalAggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    Task RemoveAsync<T, TId>(T aggregateRoot, T originalAggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull;
}