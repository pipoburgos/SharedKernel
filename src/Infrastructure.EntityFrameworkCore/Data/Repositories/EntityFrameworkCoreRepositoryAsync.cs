using SharedKernel.Application.System;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Repositories.Read.Specification;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

/// <summary> Entity Framework Core Repository. </summary>
public abstract class EntityFrameworkCoreRepositoryAsync<TAggregateRoot, TId> :
    EntityFrameworkCoreRepository<TAggregateRoot, TId>,
    IRepositoryAsync<TAggregateRoot, TId>,
    IReadAllRepositoryAsync<TAggregateRoot>,
    IReadSpecificationRepositoryAsync<TAggregateRoot>,
    ISaveRepositoryAsync where TAggregateRoot : class, IAggregateRoot
{
    #region Constructors

    /// <summary>  </summary>
    protected EntityFrameworkCoreRepositoryAsync(DbContextBase dbContextBase) : base(dbContextBase)
    {
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>  </summary>
    public virtual Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return GetQuery().Cast<IEntity<TId>>().Where(a => a.Id!.Equals(id)).Cast<TAggregateRoot>()
            .SingleOrDefaultAsync(cancellationToken)!;
    }

    /// <summary>  </summary>
    public virtual Task<bool> AnyAsync(TId id, CancellationToken cancellationToken)
    {
        return GetQuery()
            .Cast<IEntity<TId>>()
            .AnyAsync(a => a.Id!.Equals(id), cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<bool> NotAnyAsync(TId id, CancellationToken cancellationToken)
    {
        return GetQuery()
            .Cast<IEntity<TId>>()
            .AllAsync(a => !a.Id!.Equals(id), cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<TAggregateRoot?> GetDeleteByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return GetQuery(true, true)
            .Cast<IEntity<TId>>()
            .Where(a => a.Id!.Equals(id))
            .Cast<TAggregateRoot>()
            .SingleOrDefaultAsync(cancellationToken)!;
    }

    /// <summary>  </summary>
    public virtual Task<List<TAggregateRoot>> GetAllAsync(CancellationToken cancellationToken)
    {
        return GetQuery().ToListAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return GetQuery(false).AnyAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public async Task<bool> NotAnyAsync(CancellationToken cancellationToken)
    {
        return !await GetQuery(false).AnyAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return DbContextBase.SetAggregate<TAggregateRoot>().LongCountAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    {
        return GetQuery(false).Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    {
        return GetQuery().SingleAsync(spec.SatisfiedBy(), cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<TAggregateRoot?> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    {
        return GetQuery().SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken)!;
    }

    /// <summary>  </summary>
    public virtual Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    {
        return GetQuery(false).AnyAsync(spec.SatisfiedBy(), cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return DbContextBase.SetAggregate<TAggregateRoot>().AddAsync(aggregateRoot, cancellationToken).AsTask();
    }

    /// <summary>  </summary>
    public virtual Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return DbContextBase.SetAggregate<TAggregateRoot>().AddRangeAsync(aggregates, cancellationToken);
    }

    /// <summary>  </summary>
    public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().Remove(aggregateRoot);
        return TaskHelper.CompletedTask;
    }

    /// <summary>  </summary>
    public virtual Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().RemoveRange(aggregates);
        return TaskHelper.CompletedTask;
    }

    /// <summary>  </summary>
    public virtual Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().Update(aggregateRoot);
        return TaskHelper.CompletedTask;
    }

    /// <summary>  </summary>
    public virtual Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().UpdateRange(aggregates);
        return TaskHelper.CompletedTask;
    }

    /// <summary>  </summary>
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return DbContextBase.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return DbContextBase.SaveChangesResultAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return DbContextBase.RollbackAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return DbContextBase.RollbackResultAsync(cancellationToken);
    }

    #endregion
}
