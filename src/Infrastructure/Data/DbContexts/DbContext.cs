using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.Services;
// ReSharper disable SuspiciousTypeConversion.Global

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary>  </summary>
public abstract class DbContext : IDbContext
{
    /// <summary>  </summary>
    protected readonly IEntityAuditableService AuditableService;

    /// <summary>  </summary>
    protected readonly IClassValidatorService ClassValidatorService;

    /// <summary>  </summary>
    protected readonly List<IOperation> Operations;

    /// <summary>  </summary>
    protected readonly List<IOperation> OperationsExecuted;

    /// <summary>  </summary>
    public DbContext(IEntityAuditableService auditableService, IClassValidatorService classValidatorService)
    {
        AuditableService = auditableService;
        ClassValidatorService = classValidatorService;
        Operations = new List<IOperation>();
        OperationsExecuted = new List<IOperation>();
    }

    /// <summary>  </summary>
    public void Add<T, TId>(T aggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull
    {
        Operations.Add(new Operation<T, TId>(Crud.Adding, aggregateRoot, () => AddMethod<T, TId>(aggregateRoot),
            () => DeleteMethod<T, TId>(aggregateRoot)));
    }

    /// <summary>  </summary>
    public void Update<T, TId>(T aggregateRoot, T originalAggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull
    {
        Operations.Add(new Operation<T, TId>(Crud.Updating, aggregateRoot, () => UpdateMethod<T, TId>(aggregateRoot),
            () => UpdateMethod<T, TId>(originalAggregateRoot)));
    }

    /// <summary>  </summary>
    public void Remove<T, TId>(T aggregateRoot, T originalAggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull
    {
        Operations.Add(aggregateRoot is IEntityAuditableLogicalRemove
            ? new Operation<T, TId>(Crud.Deleting, aggregateRoot, () => UpdateMethod<T, TId>(aggregateRoot),
                () => UpdateMethod<T, TId>(originalAggregateRoot))
            : new Operation<T, TId>(Crud.Deleting, aggregateRoot, () => DeleteMethod<T, TId>(aggregateRoot),
                () => AddMethod<T, TId>(originalAggregateRoot)));
    }

    /// <summary>  </summary>
    public virtual int SaveChanges()
    {
        Validate();

        Audit(Operations);

        return Commit();
    }

    /// <summary>  </summary>
    public virtual Result<int> SaveChangesResult()
    {
        var addingAndUpdating = Operations.Where(x => x.Crud is Crud.Adding or Crud.Updating).ToList();

        var x1 = ClassValidatorService.ValidateDataAnnotationsResult(addingAndUpdating);

        var x2 = ClassValidatorService.ValidateValidatableObjectsResult(addingAndUpdating.OfType<IValidatableObject>());

        return Result
            .Create(Unit.Value)
            .Combine(x1, x2)
            .Tap(_ => Audit(Operations))
            .TryBind(_ => Commit());
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        var total = OperationsExecuted.Count;

        foreach (var operation in OperationsExecuted.ToList())
        {
            operation.RollbackMethod();
            OperationsExecuted.Remove(operation);
        }

        return total;
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return Rollback();
    }

    /// <summary>  </summary>
    protected abstract void AddMethod<T, TId>(T aggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    protected abstract void UpdateMethod<T, TId>(T aggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    protected abstract void DeleteMethod<T, TId>(T aggregateRoot)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    private int Commit()
    {
        var total = Operations.Count;

        BeforeCommit();

        try
        {
            foreach (var operation in Operations.ToList())
            {
                operation.CommitMethod();
                Operations.Remove(operation);
                OperationsExecuted.Add(operation);
            }
        }
        catch (Exception)
        {
            Rollback();
        }

        AfterCommit();

        return total;
    }

    /// <summary>  </summary>
    protected virtual void BeforeCommit() { }

    /// <summary>  </summary>
    protected virtual void AfterCommit() { }

    /// <summary>  </summary>
    protected void Validate()
    {
        var addingAndUpdating = Operations.Where(x => x.Crud is Crud.Adding or Crud.Updating).ToList();

        ClassValidatorService.ValidateDataAnnotations(addingAndUpdating);

        ClassValidatorService.ValidateValidatableObjects(addingAndUpdating.OfType<IValidatableObject>());
    }

    /// <summary>  </summary>
    protected virtual void Audit(IEnumerable<IOperation> operations)
    {
        var operationsList = operations.ToList();
        AuditableService.Audit(
            operationsList.Where(x => x.Crud == Crud.Adding).Select(a => a.AggregateRoot).OfType<IEntityAuditable>(),
            operationsList.Where(x => x.Crud == Crud.Updating).Select(a => a.AggregateRoot).OfType<IEntityAuditable>(),
            operationsList.Where(x => x.Crud == Crud.Deleting).Select(a => a.AggregateRoot)
                .OfType<IEntityAuditableLogicalRemove>());
    }
}
