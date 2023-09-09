using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.Services;
// ReSharper disable SuspiciousTypeConversion.Global

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>  </summary>
    protected readonly IEntityAuditableService AuditableService;

    /// <summary>  </summary>
    protected readonly IClassValidatorService ClassValidatorService;

    /// <summary>  </summary>
    protected readonly List<Action> Operations;

    /// <summary>  </summary>
    protected readonly List<IAggregateRoot> Added;

    /// <summary>  </summary>
    protected readonly List<IAggregateRoot> Modified;

    /// <summary>  </summary>
    protected readonly List<IAggregateRoot> Deleted;

    /// <summary>  </summary>
    public UnitOfWork(IEntityAuditableService auditableService, IClassValidatorService classValidatorService)
    {
        AuditableService = auditableService;
        ClassValidatorService = classValidatorService;
        Operations = new List<Action>();
        Added = new List<IAggregateRoot>();
        Modified = new List<IAggregateRoot>();
        Deleted = new List<IAggregateRoot>();
    }

    /// <summary>  </summary>
    public void AddOperation(IAggregateRoot aggregateRoot, Action operation)
    {
        Operations.Add(operation);
        Added.Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void AddOperation(IEnumerable<IAggregateRoot> aggregates, Action operation)
    {
        Operations.Add(operation);
        Added.AddRange(aggregates);
    }

    /// <summary>  </summary>
    public void UpdateOperation(IAggregateRoot aggregateRoot, Action operation)
    {
        Operations.Add(operation);
        Modified.Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public void UpdateOperation(IEnumerable<IAggregateRoot> aggregates, Action operation)
    {
        Operations.Add(operation);
        Modified.AddRange(aggregates);
    }

    /// <summary>  </summary>
    public void RemoveOperation(IAggregateRoot aggregateRoot, Action deleteOperation, Action updateOperation)
    {
        Operations.Add(aggregateRoot is IEntityAuditableLogicalRemove ? updateOperation : deleteOperation);
        Deleted.Add(aggregateRoot);
    }

    /// <summary>  </summary>
    public virtual int SaveChanges()
    {
        ClassValidatorService.ValidateDataAnnotations(Added.Concat(Modified).Concat(Deleted));

        ClassValidatorService.ValidateValidatableObjects(Added.Concat(Modified).Concat(Deleted)
            .OfType<IValidatableObject>());

        AuditableService.Audit(Added.OfType<IEntityAuditable>(), Modified.OfType<IEntityAuditable>(),
            Deleted.OfType<IEntityAuditableLogicalRemove>());

        return Commit();
    }

    /// <summary>  </summary>
    public virtual Result<int> SaveChangesResult()
    {
        return Result
            .Create(Unit.Value)
            .Combine(
                ClassValidatorService.ValidateDataAnnotationsResult(Added.Concat(Modified).Concat(Deleted)),
                ClassValidatorService.ValidateValidatableObjectsResult(Added.Concat(Modified).Concat(Deleted)
                    .OfType<IValidatableObject>()))
            .Tap(_ => AuditableService.Audit(Added.OfType<IEntityAuditable>(), Modified.OfType<IEntityAuditable>(),
                Deleted.OfType<IEntityAuditableLogicalRemove>()))
            .TryBind(_ => Commit());
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        var total = Operations.Count;
        Operations.Clear();
        Added.Clear();
        Modified.Clear();
        Deleted.Clear();
        return total;
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return Rollback();
    }

    private int Commit()
    {
        var total = Operations.Count;

        BeforeCommit();

        Operations.ForEach(o => o.Invoke());

        AfterCommit();

        Rollback();

        return total;
    }

    /// <summary>  </summary>
    protected virtual void BeforeCommit() { }

    /// <summary>  </summary>
    protected virtual void AfterCommit() { }
}
