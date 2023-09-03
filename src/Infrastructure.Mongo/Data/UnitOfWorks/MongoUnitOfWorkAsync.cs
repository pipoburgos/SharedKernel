using Microsoft.Extensions.Options;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

/// <summary>  </summary>
public class MongoUnitOfWorkAsync : MongoUnitOfWork, IUnitOfWorkAsync
{
    private readonly List<Func<Task>> _operationsAsync;

    /// <summary>  </summary>
    public MongoUnitOfWorkAsync(IOptions<MongoSettings> options, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(options, auditableService, classValidatorService)
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

        if (EnableTransactions)
            Session.StartTransaction();

        await Task.WhenAll(_operationsAsync.Select(o => o()));

        if (EnableTransactions)
            await Session.CommitTransactionAsync(cancellationToken);

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
