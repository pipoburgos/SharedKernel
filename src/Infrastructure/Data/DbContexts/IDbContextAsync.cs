using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary> . </summary>
public interface IDbContextAsync : IDbContext, IUnitOfWorkAsync
{
    /// <summary> . </summary>
    Task AddAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    Task UpdateAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    Task RemoveAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;
}