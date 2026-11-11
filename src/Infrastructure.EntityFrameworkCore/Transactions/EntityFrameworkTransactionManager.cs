#if NET || NETSTANDARD
using SharedKernel.Application.Transactions;
using System.Transactions;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Transactions;

public sealed class EntityFrameworkTransactionManager : ITransactionManager
{
    private readonly DbContext _dbContext;

    public EntityFrameworkTransactionManager(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task ExecuteAsync(Func<Task> operation)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout,
                },
                TransactionScopeAsyncFlowOption.Enabled);

            await operation();

            scope.Complete();
        });
    }
}
#endif