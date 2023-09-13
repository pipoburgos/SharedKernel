using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.DbContexts;

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public abstract class GlobalUnitOfWork : IGlobalUnitOfWork
{
    /// <summary>  </summary>
    protected readonly List<IDbContext> DbContexts;

    /// <summary>  </summary>

    protected readonly List<IDbContext> DbContextsExecuted;

    /// <summary>  </summary>
    public Guid Id { get; }

    /// <summary>  </summary>
    protected GlobalUnitOfWork(IServiceProvider serviceProvider)
    {
        Id = Guid.NewGuid();
        DbContexts = serviceProvider.GetServices<IDbContext>().ToList();
        DbContextsExecuted = new List<IDbContext>();
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        var total = 0;
        try
        {
            foreach (var dbContextAsync in DbContexts.ToList())
            {
                total += dbContextAsync.SaveChanges();
                DbContexts.Remove(dbContextAsync);
                DbContextsExecuted.Add(dbContextAsync);
            }
        }
        catch (Exception)
        {
            Rollback();
        }

        return total;
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return SaveChanges();
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        var total = 0;

        foreach (var dbContextAsync in DbContextsExecuted.ToList())
        {
            total += dbContextAsync.Rollback();
            DbContextsExecuted.Remove(dbContextAsync);
        }

        DbContextsExecuted.Clear();
        return total;
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return Rollback();
    }
}
