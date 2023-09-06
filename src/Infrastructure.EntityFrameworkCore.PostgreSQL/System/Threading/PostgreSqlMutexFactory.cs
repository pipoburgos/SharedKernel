using Medallion.Threading.Postgres;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.System.Threading;

internal class PostgreSqlMutexFactory : IMutexFactory
{
    private readonly string _connectionString;

    public PostgreSqlMutexFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDisposable Create(string key)
    {
        var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(key, true), _connectionString);
        var sqlDistributedLockHandle = @lock.Acquire();
        return sqlDistributedLockHandle;
    }

    public async Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(key, true), _connectionString);
        var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
        return sqlDistributedLockHandle;
    }
}
