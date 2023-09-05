using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Validators;
// ReSharper disable SuspiciousTypeConversion.Global

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public class UnitOfWorkAsync : UnitOfWork, IUnitOfWorkAsync
{
    private readonly List<Func<Task>> _operationsAsync;

    /// <summary>  </summary>
    public UnitOfWorkAsync(IEntityAuditableService auditableService, IClassValidatorService classValidatorService)
        : base(auditableService, classValidatorService)
    {
        _operationsAsync = new List<Func<Task>>();
    }

    /// <summary>  </summary>
    public Task AddOperationAsync(IAggregateRoot aggregateRoot, Func<Task> operation)
    {
        _operationsAsync.Add(operation);
        Added.Add(aggregateRoot);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task AddOperationAsync(IEnumerable<IAggregateRoot> aggregates, Func<Task> operation)
    {
        _operationsAsync.Add(operation);
        Added.AddRange(aggregates);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task UpdateOperationAsync(IAggregateRoot aggregateRoot, Func<Task> operation)
    {
        _operationsAsync.Add(operation);
        Modified.Add(aggregateRoot);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task UpdateOperationAsync(IEnumerable<IAggregateRoot> aggregates, Func<Task> operation)
    {
        _operationsAsync.Add(operation);
        Modified.AddRange(aggregates);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task RemoveOperationAsync(IAggregateRoot aggregateRoot, Func<Task> deleteOperation, Func<Task> updateOperation)
    {
        _operationsAsync.Add(aggregateRoot is IEntityAuditableLogicalRemove ? updateOperation : deleteOperation);
        Deleted.Add(aggregateRoot);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        ClassValidatorService.ValidateDataAnnotations(Added.Concat(Modified).Concat(Deleted));

        ClassValidatorService.ValidateValidatableObjects(Added.Concat(Modified).Concat(Deleted)
            .OfType<IValidatableObject>());

        AuditableService.Audit(Added.OfType<IEntityAuditable>(), Modified.OfType<IEntityAuditable>(),
            Deleted.OfType<IEntityAuditableLogicalRemove>());

        return CommitAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public virtual Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return Result
            .Create(Unit.Value)
            .Combine(
                ClassValidatorService.ValidateDataAnnotationsResult(Added.Concat(Modified).Concat(Deleted)),
                ClassValidatorService.ValidateValidatableObjectsResult(Added.Concat(Modified).Concat(Deleted)
                    .OfType<IValidatableObject>()))
            .Tap(_ => AuditableService.Audit(Added.OfType<IEntityAuditable>(), Modified.OfType<IEntityAuditable>(),
                Deleted.OfType<IEntityAuditableLogicalRemove>()))
            .TryBind(async _ => await CommitAsync(cancellationToken));
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        var total = Operations.Count + _operationsAsync.Count;
        Operations.Clear();
        _operationsAsync.Clear();
        Added.Clear();
        Modified.Clear();
        Deleted.Clear();
        return Task.FromResult(total);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Create(Rollback()));
    }

    private async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        var total = _operationsAsync.Count + Operations.Count;

        await BeforeCommitAsync(cancellationToken);

        Operations.ForEach(o => o.Invoke());

        await Task.WhenAll(_operationsAsync.Select(o => o()));

        await AfterCommitAsync(cancellationToken);

        await RollbackAsync(cancellationToken);

        return total;
    }

    /// <summary>  </summary>
    protected virtual Task BeforeCommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>  </summary>
    protected virtual Task AfterCommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
