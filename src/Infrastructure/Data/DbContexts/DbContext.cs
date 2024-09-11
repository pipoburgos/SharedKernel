using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.Services;
// ReSharper disable SuspiciousTypeConversion.Global

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary> . </summary>
public abstract class DbContext : IDbContext
{
    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    protected readonly IEntityAuditableService AuditableService;

    /// <summary> . </summary>
    protected readonly IClassValidatorService ClassValidatorService;

    /// <summary> . </summary>
    protected readonly List<IOperation> Operations;

    /// <summary> . </summary>
    protected readonly List<IOperation> OperationsExecuted;

    /// <summary> . </summary>
    protected DbContext(IEntityAuditableService auditableService, IClassValidatorService classValidatorService)
    {
        Id = Guid.NewGuid();
        AuditableService = auditableService;
        ClassValidatorService = classValidatorService;
        Operations = new List<IOperation>();
        OperationsExecuted = new List<IOperation>();
    }

    /// <summary> . </summary>
    public void Add<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        Operations.Add(new Operation<TAggregateRoot, TId>(Crud.Adding, aggregateRoot, () => AddMethod<TAggregateRoot, TId>(aggregateRoot),
            () => DeleteMethod<TAggregateRoot, TId>(aggregateRoot)));
    }

    /// <summary> . </summary>
    public void Update<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        Operations.Add(new Operation<TAggregateRoot, TId>(Crud.Updating, aggregateRoot,
            () => UpdateMethod<TAggregateRoot, TId>(aggregateRoot),
            () => UpdateMethod<TAggregateRoot, TId>(GetById<TAggregateRoot, TId>(aggregateRoot.Id)!)));
    }

    /// <summary> . </summary>
    public void Remove<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        Operations.Add(aggregateRoot is IEntityAuditableLogicalRemove
            ? new Operation<TAggregateRoot, TId>(Crud.Deleting, aggregateRoot,
                () => UpdateMethod<TAggregateRoot, TId>(aggregateRoot),
                () => UpdateMethod<TAggregateRoot, TId>(GetById<TAggregateRoot, TId>(aggregateRoot.Id)!))
            : new Operation<TAggregateRoot, TId>(Crud.Deleting, aggregateRoot,
                () => DeleteMethod<TAggregateRoot, TId>(aggregateRoot),
                () => AddMethod<TAggregateRoot, TId>(GetById<TAggregateRoot, TId>(aggregateRoot.Id)!)));
    }

    /// <summary> . </summary>
    public virtual int SaveChanges()
    {
        Validate();

        Audit(Operations);

        return Commit();
    }

    /// <summary> . </summary>
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

    /// <summary> . </summary>
    public virtual int Rollback()
    {
        var total = OperationsExecuted.Count;

        foreach (var operation in OperationsExecuted.ToList())
        {
            operation.RollbackMethod();
            OperationsExecuted.Remove(operation);
        }

        Operations.Clear();

        return total;
    }

    /// <summary> . </summary>
    public virtual Result<int> RollbackResult()
    {
        return Rollback();
    }

    /// <summary> . </summary>
    protected abstract void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    protected abstract void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    protected abstract void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

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

    /// <summary> . </summary>
    protected virtual void BeforeCommit() { }

    /// <summary> . </summary>
    protected virtual void AfterCommit() { }

    /// <summary> . </summary>
    protected void Validate()
    {
        var addingAndUpdating = Operations.Where(x => x.Crud is Crud.Adding or Crud.Updating).ToList();

        ClassValidatorService.ValidateDataAnnotations(addingAndUpdating);

        ClassValidatorService.ValidateValidatableObjects(addingAndUpdating.OfType<IValidatableObject>());
    }

    /// <summary> . </summary>
    protected virtual void Audit(IEnumerable<IOperation> operations)
    {
        var operationsList = operations.ToList();
        AuditableService.Audit(
            operationsList.Where(x => x.Crud == Crud.Adding).Select(a => a.AggregateRoot).OfType<IEntityAuditable>(),
            operationsList.Where(x => x.Crud == Crud.Updating).Select(a => a.AggregateRoot).OfType<IEntityAuditable>(),
            operationsList.Where(x => x.Crud == Crud.Deleting).Select(a => a.AggregateRoot)
                .OfType<IEntityAuditableLogicalRemove>());
    }

    /// <summary> . </summary>
    public abstract TAggregateRoot? GetById<TAggregateRoot, TId>(TId id)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;
}
