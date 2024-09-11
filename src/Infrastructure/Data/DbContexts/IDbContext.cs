using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary> . </summary>
public interface IDbContext : IUnitOfWork
{
    /// <summary> . </summary>
    void Add<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    void Update<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    void Remove<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    TAggregateRoot? GetById<TAggregateRoot, TId>(TId id)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;
}