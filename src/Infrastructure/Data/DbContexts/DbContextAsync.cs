using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;
using SharedKernel.Infrastructure.Data.Services;
// ReSharper disable SuspiciousTypeConversion.Global

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary>  </summary>
public abstract class DbContextAsync : DbContext, IDbContextAsync
{
    /// <summary>  </summary>
    protected readonly List<IOperationAsync> OperationsAsync;

    /// <summary>  </summary>
    protected readonly List<IOperationAsync> OperationsExecutedAsync;

    /// <summary>  </summary>
    public DbContextAsync(IEntityAuditableService auditableService, IClassValidatorService classValidatorService)
        : base(auditableService, classValidatorService)
    {
        OperationsAsync = new List<IOperationAsync>();
        OperationsExecutedAsync = new List<IOperationAsync>();
    }


    /// <summary>  </summary>
    public Task AddAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull
    {
        OperationsAsync.Add(new OperationAsync<T, TId>(Crud.Adding, aggregateRoot,
            () => AddMethodAsync<T, TId>(aggregateRoot, cancellationToken),
            () => DeleteMethodAsync<T, TId>(aggregateRoot, cancellationToken)));

        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task UpdateAsync<T, TId>(T aggregateRoot, T originalAggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull
    {
        OperationsAsync.Add(new OperationAsync<T, TId>(Crud.Updating, aggregateRoot,
            () => UpdateMethodAsync<T, TId>(aggregateRoot, cancellationToken),
            () => UpdateMethodAsync<T, TId>(originalAggregateRoot, cancellationToken)));

        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task RemoveAsync<T, TId>(T aggregateRoot, T originalAggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull
    {
        OperationsAsync.Add(aggregateRoot is IEntityAuditableLogicalRemove
            ? new OperationAsync<T, TId>(Crud.Deleting, aggregateRoot,
                () => UpdateMethodAsync<T, TId>(aggregateRoot, cancellationToken),
                () => UpdateMethodAsync<T, TId>(originalAggregateRoot, cancellationToken))
            : new OperationAsync<T, TId>(Crud.Deleting, aggregateRoot,
                () => DeleteMethodAsync<T, TId>(aggregateRoot, cancellationToken),
                () => AddMethodAsync<T, TId>(originalAggregateRoot, cancellationToken)));

        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var addingAndUpdating = Operations.Where(x => x.Crud == Crud.Adding || x.Crud == Crud.Updating).ToList();

        ClassValidatorService.ValidateDataAnnotations(addingAndUpdating);

        ClassValidatorService.ValidateValidatableObjects(addingAndUpdating.OfType<IValidatableObject>());

        AuditableService.Audit(
            Operations.Where(x => x.Crud == Crud.Adding).OfType<IEntityAuditable>(),
            Operations.Where(x => x.Crud == Crud.Updating).OfType<IEntityAuditable>(),
            Operations.Where(x => x.Crud == Crud.Deleting).OfType<IEntityAuditableLogicalRemove>());

        return CommitAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        var addingAndUpdating = Operations.Where(x => x.Crud == Crud.Adding || x.Crud == Crud.Updating).ToList();

        var x1 = ClassValidatorService.ValidateDataAnnotationsResult(addingAndUpdating);

        var x2 = ClassValidatorService.ValidateValidatableObjectsResult(addingAndUpdating.OfType<IValidatableObject>());

        return Result
            .Create(Unit.Value)
            .Combine(x1, x2)
            .Tap(_ => AuditableService.Audit(
                Operations.Where(x => x.Crud == Crud.Adding).OfType<IEntityAuditable>(),
                Operations.Where(x => x.Crud == Crud.Updating).OfType<IEntityAuditable>(),
                Operations.Where(x => x.Crud == Crud.Deleting).OfType<IEntityAuditableLogicalRemove>()))
            .TryBind(async _ => await CommitAsync(cancellationToken));
    }

    /// <summary>  </summary>
    public async Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        var total = OperationsExecuted.Count + OperationsAsync.Count;

        foreach (var operation in OperationsExecuted)
        {
            operation.RollbackMethod();
            OperationsExecuted.Remove(operation);
            OperationsExecuted.Add(operation);
        }

        foreach (var operationAsync in OperationsExecutedAsync)
        {
            await operationAsync.RollbackMethodAsync();
            OperationsExecutedAsync.Remove(operationAsync);
            OperationsExecutedAsync.Add(operationAsync);
        }

        return total;
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Create(Rollback()));
    }

    private async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        var total = Operations.Count;

        await BeforeCommitAsync(cancellationToken);

        try
        {
            foreach (var operation in Operations.ToList())
            {
                operation.CommitMethod();
                Operations.Remove(operation);
                OperationsExecuted.Add(operation);
            }

            foreach (var operationAsync in OperationsExecutedAsync)
            {
                await operationAsync.CommitMethodAsync();
                OperationsAsync.Remove(operationAsync);
                OperationsAsync.Add(operationAsync);
            }
        }
        finally
        {
            Rollback();
        }

        await AfterCommitAsync(cancellationToken);

        await RollbackAsync(cancellationToken);

        return total;
    }

    /// <summary>  </summary>
    protected virtual Task BeforeCommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>  </summary>
    protected virtual Task AfterCommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>  </summary>
    protected abstract Task AddMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    protected abstract Task UpdateMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull;

    /// <summary>  </summary>
    protected abstract Task DeleteMethodAsync<T, TId>(T aggregateRoot, CancellationToken cancellationToken)
        where T : class, IAggregateRoot<TId> where TId : notnull;
}
