using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Repositories.Read.Specification;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

/// <summary> Entity Framework Core Repository. </summary>
public abstract class EntityFrameworkCoreRepository<TAggregateRoot, TId> :
    IRepository<TAggregateRoot, TId>,
    IReadAllRepository<TAggregateRoot>,
    IReadSpecificationRepository<TAggregateRoot>,
    ISaveRepository
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    #region Members

    /// <summary> Db context base </summary>
    protected readonly EntityFrameworkDbContext EntityFrameworkDbContext;

    #endregion Members

    #region Constructors

    /// <summary> Constructor. </summary>
    protected EntityFrameworkCoreRepository(EntityFrameworkDbContext entityFrameworkDbContext)
    {
        EntityFrameworkDbContext = entityFrameworkDbContext ?? throw new ArgumentNullException(nameof(entityFrameworkDbContext));
    }

    #endregion Constructors

    #region Protected Methods

    /// <summary>  </summary>
    protected IQueryable<TAggregateRoot> GetQuery(bool tracking = true, bool showDeleted = false,
        EntityFrameworkDbContext? dbContextBase = default)
    {
        IQueryable<TAggregateRoot> query = (dbContextBase ?? EntityFrameworkDbContext).Set<TAggregateRoot>();

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
    public virtual TAggregateRoot GetById(TId id)
    {
        return GetQuery().Where(a => a.Id!.Equals(id)).SingleOrDefault()!;
    }

    /// <summary>  </summary>
    public List<TAggregateRoot> GetAll()
    {
        return GetQuery().ToList();
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
    public virtual bool Any(TId id)
    {
        return GetQuery(false).Cast<IEntity<TId>>().Where(a => a.Id!.Equals(id)).Cast<TAggregateRoot>().Any();
    }

    /// <summary>  </summary>
    public bool NotAny(TId id)
    {
        return !GetQuery(false).Cast<IEntity<TId>>().Where(a => a.Id!.Equals(id)).Cast<TAggregateRoot>().Any();
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
        EntityFrameworkDbContext.Set<TAggregateRoot>().Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual void AddRange(IEnumerable<TAggregateRoot> aggregateRoots)
    {
        EntityFrameworkDbContext.Set<TAggregateRoot>().AddRange(aggregateRoots);
    }

    /// <summary>  </summary>
    public virtual void Remove(TAggregateRoot aggregateRoot)
    {
        EntityFrameworkDbContext.Set<TAggregateRoot>().Remove(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        EntityFrameworkDbContext.Set<TAggregateRoot>().RemoveRange(aggregates);
    }

    /// <summary>  </summary>
    public virtual void Update(TAggregateRoot aggregateRoot)
    {
        EntityFrameworkDbContext.Set<TAggregateRoot>().Update(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        EntityFrameworkDbContext.Set<TAggregateRoot>().UpdateRange(aggregates);
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        return EntityFrameworkDbContext.SaveChanges();
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return EntityFrameworkDbContext.SaveChangesResult();
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        return EntityFrameworkDbContext.Rollback();
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return EntityFrameworkDbContext.Rollback();
    }

    #endregion
}
