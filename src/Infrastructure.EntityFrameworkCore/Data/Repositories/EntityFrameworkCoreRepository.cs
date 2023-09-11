using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Repositories.Read.Specification;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

/// <summary> Entity Framework Core Repository. </summary>
public abstract class EntityFrameworkCoreRepository<TAggregateRoot, TId> : Repository<TAggregateRoot, TId>,
    IReadAllRepository<TAggregateRoot>,
    IReadSpecificationRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    #region Members

    /// <summary> Db context base </summary>
    protected readonly EntityFrameworkDbContext EntityFrameworkDbContext;

    #endregion Members

    #region Constructors

    /// <summary> Constructor. </summary>
    protected EntityFrameworkCoreRepository(EntityFrameworkDbContext entityFrameworkDbContext) : base(entityFrameworkDbContext)
    {
        EntityFrameworkDbContext = entityFrameworkDbContext;
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

    /// <summary>  </summary>
    public override TAggregateRoot GetById(TId id)
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
    public virtual bool NotAny()
    {
        return !GetQuery(false).Any();
    }

    /// <summary>  </summary>
    public override bool Any(TId id)
    {
        return GetQuery(false).Where(a => a.Id!.Equals(id)).Any();
    }

    /// <summary>  </summary>
    public override bool NotAny(TId id)
    {
        return !GetQuery(false).Where(a => a.Id!.Equals(id)).Any();
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
}
