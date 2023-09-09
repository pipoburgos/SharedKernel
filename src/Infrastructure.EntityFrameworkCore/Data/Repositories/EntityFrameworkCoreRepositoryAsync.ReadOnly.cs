using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Repositories.Read.Specification;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

/// <summary> Entity Framework Core Repository. </summary>
public abstract class EntityFrameworkCoreRepositoryAsync<TAggregateRoot, TId, TDbContext> :
    EntityFrameworkCoreRepositoryAsync<TAggregateRoot, TId>,
    IReadOnlyRepositoryAsync<TAggregateRoot, TId>,
    IReadOnlySpecificationRepositoryAsync<TAggregateRoot>,
    IDisposable where TAggregateRoot : class, IAggregateRoot<TId> where TDbContext : DbContextBase where TId : notnull
{

    #region Members

    /// <summary> Db Context Factory for concurrence async await operations. </summary>
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    private readonly List<TDbContext> _contexts;

    #endregion Members

    #region Constructors

    /// <summary>  </summary>
    protected EntityFrameworkCoreRepositoryAsync(TDbContext dbContextBase,
        IDbContextFactory<TDbContext> dbContextFactory) : base(dbContextBase)
    {
        _dbContextFactory = dbContextFactory;
        _contexts = new List<TDbContext>();
    }

    #endregion Constructors

    /// <summary> Creates a new DbContext an invoke Set method with no tracking. </summary>
    protected IQueryable<TEntity> SetReadOnly<TEntity>() where TEntity : class
    {
        var context = _dbContextFactory.CreateDbContext();
        _contexts.Add(context);
        return context.Set<TEntity>().AsNoTracking();
    }

    /// <summary> Creates a new DbContext an get aggregate with no tracking. </summary>
    protected IQueryable<TAggregateRoot> GetReadOnlyQuery(bool showDeleted = false)
    {
        var context = _dbContextFactory.CreateDbContext();
        _contexts.Add(context);
        return GetQuery(false, showDeleted, context);
    }

    /// <summary>  </summary>
    public virtual Task<TAggregateRoot?> GetByIdReadOnlyAsync(TId id, CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery()
            .Cast<IEntity<TId>>()
            .Where(a => a.Id!.Equals(id))
            .Cast<TAggregateRoot>()
            .SingleOrDefaultAsync(cancellationToken)!;
    }

    /// <summary>  </summary>
    public virtual Task<TAggregateRoot?> GetDeleteByIdReadOnlyAsync(TId id,
        CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery(true)
            .Cast<IEntity<TId>>()
            .Where(a => a.Id!.Equals(id))
            .Cast<TAggregateRoot>()
            .SingleOrDefaultAsync(cancellationToken)!;
    }

    /// <summary>  </summary>
    public Task<List<TAggregateRoot>> GetAllReadOnlyAsync(CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().ToListAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public override Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().AnyAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public override Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().LongCountAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public override Task<bool> AnyAsync(TId id, CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery()
            .Cast<IEntity<TId>>()
            .AnyAsync(a => a.Id!.Equals(id), cancellationToken);
    }

    /// <summary>  </summary>
    public Task<List<TAggregateRoot>> WhereReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<TAggregateRoot> SingleReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().SingleAsync(spec.SatisfiedBy(), cancellationToken);
    }

    /// <summary>  </summary>
    public Task<TAggregateRoot?> SingleOrDefaultReadOnlyAsync(ISpecification<TAggregateRoot> spec,
        CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken)!;
    }

    /// <summary>  </summary>
    public override Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    {
        return GetReadOnlyQuery().AnyAsync(spec.SatisfiedBy(), cancellationToken);
    }

    /// <summary>  </summary>
    public void Dispose()
    {
        foreach (var dbContextBase in _contexts)
        {
            dbContextBase.Dispose();
        }
    }
}
