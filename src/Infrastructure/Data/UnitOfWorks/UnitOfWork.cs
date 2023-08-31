using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IIdentityService _identityService;
    private readonly IDateTime _dateTime;
    private readonly List<Action> _operations;
    private readonly List<IAggregateRoot> _added;
    private readonly List<IAggregateRoot> _modified;
    private readonly List<IAggregateRoot> _deleted;

    /// <summary>  </summary>
    public UnitOfWork(IIdentityService identityService, IDateTime dateTime)
    {
        _identityService = identityService;
        _dateTime = dateTime;
        _operations = new List<Action>();
        _added = new List<IAggregateRoot>();
        _modified = new List<IAggregateRoot>();
        _deleted = new List<IAggregateRoot>();
    }

    /// <summary>  </summary>
    public void AddOperation(Action operation, IAggregateRoot aggregateRoot)
    {
        _operations.Add(operation);
        _added.Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void AddOperation(Action operation, IEnumerable<IAggregateRoot> aggregates)
    {
        _operations.Add(operation);
        _added.AddRange(aggregates);
    }

    /// <summary>  </summary>
    public void UpdateOperation(Action operation, IAggregateRoot aggregateRoot)
    {
        _operations.Add(operation);
        _modified.Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void UpdateOperation(Action operation, IEnumerable<IAggregateRoot> aggregates)
    {
        _operations.Add(operation);
        _modified.AddRange(aggregates);
    }

    /// <summary>  </summary>
    public void RemoveOperation(Action operation, IAggregateRoot aggregateRoot)
    {
        _operations.Add(operation);
        _deleted.Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void RemoveOperation(Action operation, IEnumerable<IAggregateRoot> aggregates)
    {
        _operations.Add(operation);
        _deleted.AddRange(aggregates);
    }

    /// <summary>  </summary>
    public virtual int SaveChanges()
    {
        var total = _operations.Count;

        SetAuditableAggregatesValues();

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

    private void SetAuditableAggregatesValues()
    {
        var now = _dateTime.UtcNow;
        var user = _identityService.UserId;

        foreach (var added in _added.OfType<IEntityAuditable>())
        {
            added.Create(now, user);
        }

        foreach (var modified in _modified.OfType<IEntityAuditable>())
        {
            modified.Change(now, user);
        }

        foreach (var removed in _deleted.OfType<IEntityAuditableLogicalRemove>())
        {
            removed.Delete(now, user);
        }
    }
}
