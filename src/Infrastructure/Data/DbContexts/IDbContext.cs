using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary>  </summary>
public interface IDbContext : IUnitOfWork
{
    /// <summary>  </summary>
    void Add<T, TId>(T aggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    void Update<T, TId>(T aggregateRoot, T originalAggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    void Remove<T, TId>(T aggregateRoot, T originalAggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull;
}