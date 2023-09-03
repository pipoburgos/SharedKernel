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
    public Task AddOperationAsync(Func<Task> operation)
    {
        _operationsAsync.Add(operation);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public Task AddOperationAsync<T>(Func<Task<T>> operation)
    {
        _operationsAsync.Add(operation);
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        ClassValidatorService.ValidateDataAnnotations(Added.Concat(Modified).Concat(Deleted));

        ClassValidatorService.ValidateValidatableObjects(Added.Concat(Modified).Concat(Deleted)
            .OfType<IValidatableObject>());

        AuditableService.Audit(Added.OfType<IEntityAuditable>(), Modified.OfType<IEntityAuditable>(),
            Deleted.OfType<IEntityAuditableLogicalRemove>());

        var total = _operationsAsync.Count + Operations.Count;

        Operations.ForEach(o => o.Invoke());

        await Task.WhenAll(_operationsAsync.Select(o => o()));

        await RollbackAsync(cancellationToken);

        return total;
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return Result
            .Create(Unit.Value)
            .Combine(
                ClassValidatorService.ValidateDataAnnotationsResult(Added.Concat(Modified).Concat(Deleted)),
                ClassValidatorService.ValidateValidatableObjectsResult(Added.Concat(Modified).Concat(Deleted)
                    .OfType<IValidatableObject>()))
            .Tap(_ => AuditableService.Audit(Added.OfType<IEntityAuditable>(), Modified.OfType<IEntityAuditable>(),
                Deleted.OfType<IEntityAuditableLogicalRemove>()))
            .TryBind(async _ =>
            {
                var total = Operations.Count;

                Operations.ForEach(o => o.Invoke());

                await Task.WhenAll(_operationsAsync.Select(o => o()));

                await RollbackAsync(cancellationToken);

                return total;
            });
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        var total = Operations.Count + _operationsAsync.Count;
        Operations.Clear();
        _operationsAsync.Clear();
        return Task.FromResult(total);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Create(Rollback()));
    }
}
