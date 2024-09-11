using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Infrastructure.Data.DbContexts;

namespace SharedKernel.Infrastructure.Data.Repositories;

/// <summary> . </summary>
public abstract class Repository<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId>,
    ISaveRepository where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    private readonly IDbContext _dbContext;

    /// <summary> . </summary>
    protected Repository(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary> . </summary>
    public virtual void Add(TAggregateRoot aggregateRoot)
    {
        _dbContext.Add<TAggregateRoot, TId>(aggregateRoot);
    }

    /// <summary> . </summary>
    public virtual void AddRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Add(aggregateRoot);
        }
    }

    /// <summary> . </summary>
    public virtual TAggregateRoot? GetById(TId id)
    {
        return _dbContext.GetById<TAggregateRoot, TId>(id);
    }

    /// <summary> . </summary>
    public virtual bool Any(TId id)
    {
        return GetById(id) != default;
    }

    /// <summary> . </summary>
    public virtual bool NotAny(TId id)
    {
        return GetById(id) == default;
    }

    /// <summary> . </summary>
    public virtual void Update(TAggregateRoot aggregateRoot)
    {
        _dbContext.Update<TAggregateRoot, TId>(aggregateRoot);
    }

    /// <summary> . </summary>
    public virtual void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }

    /// <summary> . </summary>
    public virtual void Remove(TAggregateRoot aggregateRoot)
    {
        _dbContext.Remove<TAggregateRoot, TId>(aggregateRoot);
    }

    /// <summary> . </summary>
    public virtual void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Remove(aggregateRoot);
        }
    }

    /// <summary> . </summary>
    public virtual int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    /// <summary> . </summary>
    public virtual Result<int> SaveChangesResult()
    {
        return _dbContext.SaveChangesResult();
    }

    /// <summary> . </summary>
    public virtual int Rollback()
    {
        return _dbContext.Rollback();
    }

    /// <summary> . </summary>
    public virtual Result<int> RollbackResult()
    {
        return _dbContext.RollbackResult();
    }
}