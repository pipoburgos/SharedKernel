using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Application.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public class UnitOfWorkAsync : UnitOfWork, IUnitOfWorkAsync
{
    private readonly List<Func<Task>> _operationsAsync;

    /// <summary>  </summary>
    public UnitOfWorkAsync(IIdentityService identityService, IDateTime dateTime) : base(identityService, dateTime)
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
        var total = _operationsAsync.Count;

        await Task.WhenAll(_operationsAsync.Select(o => o()));

        await RollbackAsync(cancellationToken);

        return total;
    }

    /// <summary>  </summary>
    public async Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return await SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Rollback());
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Create(Rollback()));
    }
}
