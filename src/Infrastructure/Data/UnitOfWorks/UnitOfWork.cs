using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly List<Action> _operations;

    /// <summary>  </summary>
    public UnitOfWork()
    {
        _operations = new List<Action>();
    }

    /// <summary>  </summary>
    public void AddOperation(Action operation)
    {
        _operations.Add(operation);
    }

    /// <summary>  </summary>
    public virtual int SaveChanges()
    {
        var total = _operations.Count;

        _operations.ForEach(o => o.Invoke());

        Rollback();

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
        var total = _operations.Count;
        _operations.Clear();
        return total;
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return Rollback();
    }
}
