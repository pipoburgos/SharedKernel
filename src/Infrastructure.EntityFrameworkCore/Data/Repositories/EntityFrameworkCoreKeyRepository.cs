using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

/// <summary> Entity Framework Core Repository. </summary>
/// <typeparam name="TAggregateRoot"> Repository data type.</typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class EntityFrameworkCoreRepository<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot
{
    #region Members

    /// <summary> Db context base </summary>
    protected readonly DbContextBase DbContextBase;

    #endregion Members

    #region Constructors

    /// <summary> Constructor. </summary>
    protected EntityFrameworkCoreRepository(DbContextBase dbContextBase)
    {
        DbContextBase = dbContextBase ?? throw new ArgumentNullException(nameof(dbContextBase));
    }

    #endregion Constructors

    #region Protected Methods

    /// <summary>  </summary>
    protected IQueryable<TAggregateRoot> GetQuery(bool tracking = true, bool showDeleted = false,
        DbContextBase? dbContextBase = default)
    {
        IQueryable<TAggregateRoot> query = (dbContextBase ?? DbContextBase).SetAggregate<TAggregateRoot>();

        query = GetAggregate(query);

        if (typeof(IEntityIsTranslatable<>).IsAssignableFrom(typeof(TAggregateRoot)))
        {
            query = query
                .Cast<IEntityIsTranslatable<dynamic>>()
                .Include(a => a.Translations)
                .Cast<TAggregateRoot>();
        }

        if (!showDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(TAggregateRoot)))
        {
            query = query
                .Cast<IEntityAuditableLogicalRemove>()
                .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                .Cast<TAggregateRoot>();
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (tracking)
            return query;

        return query.AsNoTracking();
    }

    /// <summary>  </summary>
    protected virtual IQueryable<TAggregateRoot> GetAggregate(IQueryable<TAggregateRoot> query)
    {
        return query;
    }

    #endregion

    #region Public Methods

    /// <summary>  </summary>
    public virtual TAggregateRoot GetById(TId key)
    {
        return GetQuery().Cast<IEntity<TId>>().Where(a => a.Id!.Equals(key)).Cast<TAggregateRoot>().SingleOrDefault()!;
    }

    /// <summary>  </summary>
    public virtual bool Any()
    {
        return GetQuery(false).Any();
    }

    /// <summary>  </summary>
    public bool NotAny()
    {
        return !GetQuery(false).Any();
    }

    /// <summary>  </summary>
    public virtual bool Any(TId key)
    {
        return GetQuery(false).Cast<IEntity<TId>>().Where(a => a.Id!.Equals(key)).Cast<TAggregateRoot>().Any();
    }

    /// <summary>  </summary>
    public bool NotAny(TId key)
    {
        return !GetQuery(false).Cast<IEntity<TId>>().Where(a => a.Id!.Equals(key)).Cast<TAggregateRoot>().Any();
    }

    /// <summary>  </summary>
    public virtual List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Where(spec.SatisfiedBy()).ToList();
    }

    /// <summary>  </summary>

    public virtual TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Single(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public virtual TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().SingleOrDefault(spec.SatisfiedBy())!;
    }

    /// <summary>  </summary>
    public virtual bool Any(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery(false).Any(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public bool NotAny(ISpecification<TAggregateRoot> spec)
    {
        return !GetQuery(false).Any(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public virtual void Add(TAggregateRoot aggregateRoot)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual void AddRange(IEnumerable<TAggregateRoot> aggregateRoots)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().AddRange(aggregateRoots);
    }

    /// <summary>  </summary>
    public virtual void Remove(TAggregateRoot aggregateRoot)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().Remove(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().RemoveRange(aggregates);
    }

    /// <summary>  </summary>
    public virtual void Update(TAggregateRoot aggregateRoot)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().Update(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        DbContextBase.SetAggregate<TAggregateRoot>().UpdateRange(aggregates);
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        return DbContextBase.SaveChanges();
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return DbContextBase.SaveChangesResult();
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        return DbContextBase.Rollback();
    }

    #endregion
}
