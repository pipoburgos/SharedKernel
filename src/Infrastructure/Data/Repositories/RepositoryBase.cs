using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Infrastructure.Data.DbContexts;

namespace SharedKernel.Infrastructure.Data.Repositories;

/// <summary>  </summary>
public abstract class RepositoryBase<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId>,
    ISaveRepository where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    private readonly IDbContext _dbContext;

    /// <summary>  </summary>
    protected RepositoryBase(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>  </summary>
    public void Add(TAggregateRoot aggregateRoot)
    {
        _dbContext.Add<TAggregateRoot, TId>(aggregateRoot);
    }

    /// <summary>  </summary>
    public void AddRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Add(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public abstract TAggregateRoot? GetById(TId id);

    /// <summary>  </summary>
    public bool Any(TId id)
    {
        return GetById(id) != default;
    }

    /// <summary>  </summary>
    public bool NotAny(TId id)
    {
        return GetById(id) == default;
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot aggregateRoot)
    {
        _dbContext.Update<TAggregateRoot, TId>(aggregateRoot, GetById(aggregateRoot.Id)!);
    }

    /// <summary>  </summary>
    public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public void Remove(TAggregateRoot aggregateRoot)
    {
        _dbContext.Remove<TAggregateRoot, TId>(aggregateRoot, GetById(aggregateRoot.Id)!);
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Remove(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return _dbContext.SaveChangesResult();
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        return _dbContext.Rollback();
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return _dbContext.RollbackResult();
    }
}