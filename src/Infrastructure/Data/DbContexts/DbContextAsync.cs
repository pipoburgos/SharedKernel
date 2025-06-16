using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.Services;
// ReSharper disable SuspiciousTypeConversion.Global

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary> . </summary>
public abstract class DbContextAsync : DbContext, IDbContextAsync
{
    /// <summary> . </summary>
    protected readonly List<IOperationAsync> OperationsAsync;

    /// <summary> . </summary>
    protected readonly List<IOperationAsync> OperationsExecutedAsync;

    /// <summary> . </summary>
    public DbContextAsync(IEntityAuditableService auditableService, IClassValidatorService classValidatorService)
        : base(auditableService, classValidatorService)
    {
        OperationsAsync = new List<IOperationAsync>();
        OperationsExecutedAsync = new List<IOperationAsync>();
    }

    /// <summary> . </summary>
    public Task AddAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        OperationsAsync.Add(new OperationAsync<TAggregateRoot, TId>(Crud.Adding, aggregateRoot,
            () => AddMethodAsync<TAggregateRoot, TId>(aggregateRoot, cancellationToken),
            () => DeleteMethodAsync<TAggregateRoot, TId>(aggregateRoot, cancellationToken)));

        return Task.CompletedTask;
    }

    /// <summary> . </summary>
    public Task UpdateAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        OperationsAsync.Add(new OperationAsync<TAggregateRoot, TId>(Crud.Updating, aggregateRoot,
            () => UpdateMethodAsync<TAggregateRoot, TId>(aggregateRoot, cancellationToken),
            () => UpdateMethodAsync<TAggregateRoot, TId>(GetById<TAggregateRoot, TId>(aggregateRoot.Id)!,
                cancellationToken)));

        return Task.CompletedTask;
    }

    /// <summary> . </summary>
    public Task RemoveAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
    {
        OperationsAsync.Add(aggregateRoot is IEntityAuditableLogicalRemove
            ? new OperationAsync<TAggregateRoot, TId>(Crud.Deleting, aggregateRoot,
                () => UpdateMethodAsync<TAggregateRoot, TId>(aggregateRoot, cancellationToken),
                () => UpdateMethodAsync<TAggregateRoot, TId>(GetById<TAggregateRoot, TId>(aggregateRoot.Id)!,
                    cancellationToken))
            : new OperationAsync<TAggregateRoot, TId>(Crud.Deleting, aggregateRoot,
                () => DeleteMethodAsync<TAggregateRoot, TId>(aggregateRoot, cancellationToken),
                () => AddMethodAsync<TAggregateRoot, TId>(GetById<TAggregateRoot, TId>(aggregateRoot.Id)!,
                    cancellationToken)));

        return Task.CompletedTask;
    }

    /// <summary> . </summary>
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        Validate();

        Audit(Operations, OperationsAsync);

        return CommitAsync(cancellationToken);
    }

    /// <summary> . </summary>
    public virtual Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        var addingAndUpdating = Operations.Where(x => x.Crud == Crud.Adding || x.Crud == Crud.Updating).ToList();

        var x1 = ClassValidatorService.ValidateDataAnnotationsResult(addingAndUpdating);

        var x2 = ClassValidatorService.ValidateValidatableObjectsResult(addingAndUpdating.OfType<IValidatableObject>());

        return Result
            .Create(Unit.Value)
            .Combine(x1, x2)
            .Tap(_ => Audit(Operations, OperationsAsync))
            .TryBind(async _ => await CommitAsync(cancellationToken));
    }

    /// <summary> . </summary>
    public async Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        var total = OperationsExecuted.Count + OperationsExecutedAsync.Count;

        foreach (var operation in OperationsExecuted.ToList())
        {
            operation.RollbackMethod();
            OperationsExecuted.Remove(operation);
        }

        foreach (var operationAsync in OperationsExecutedAsync.ToList())
        {
            await operationAsync.RollbackMethodAsync();
            OperationsExecutedAsync.Remove(operationAsync);
        }

        Operations.Clear();
        OperationsAsync.Clear();

        return total;
    }

    /// <summary> . </summary>
    public virtual async Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        var total = await RollbackAsync(cancellationToken);
        return Result.Create(total);
    }

    private async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        var total = Operations.Count + OperationsAsync.Count;

        await BeforeCommitAsync(cancellationToken);

        try
        {
            foreach (var operation in Operations.ToList())
            {
                operation.CommitMethod();
                Operations.Remove(operation);
                OperationsExecuted.Add(operation);
            }

            foreach (var operationAsync in OperationsAsync.ToList())
            {
                await operationAsync.CommitMethodAsync();
                OperationsAsync.Remove(operationAsync);
                OperationsExecutedAsync.Add(operationAsync);
            }
        }
        catch (Exception)
        {
            await RollbackAsync(cancellationToken);
            throw;
        }

        await AfterCommitAsync(cancellationToken);

        return total;
    }

    /// <summary> . </summary>
    protected virtual Task BeforeCommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary> . </summary>
    protected virtual Task AfterCommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary> . </summary>
    protected abstract Task AddMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    protected abstract Task UpdateMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    protected abstract Task DeleteMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken) where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary> . </summary>
    protected void Audit(IEnumerable<IOperation> operations, IEnumerable<IOperationAsync> operationsAsync)
    {
        base.Audit(operations);
        var operationsList = operationsAsync.ToList();
        AuditableService.Audit(
            operationsList.Where(x => x.Crud == Crud.Adding).Select(a => a.AggregateRoot).OfType<IEntityAuditable>(),
            operationsList.Where(x => x.Crud == Crud.Updating).Select(a => a.AggregateRoot).OfType<IEntityAuditable>(),
            operationsList.Where(x => x.Crud == Crud.Deleting).Select(a => a.AggregateRoot)
                .OfType<IEntityAuditableLogicalRemove>());
    }

    /// <summary> . </summary>
    public abstract Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id, CancellationToken cancellationToken)
        where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull;
}
