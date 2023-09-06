using Medallion.Threading.SqlServer;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.System.Threading;

internal class SqlServerMutexFactory : IMutexFactory
{
    private readonly string _connectionString;

    public SqlServerMutexFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDisposable Create(string key)
    {
        var @lock = new SqlDistributedLock(key, _connectionString);
        var sqlDistributedLockHandle = @lock.Acquire();
        return sqlDistributedLockHandle;
    }

    public async Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var @lock = new SqlDistributedLock(key, _connectionString);
        var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
        return sqlDistributedLockHandle;
    }
}
