using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading.SqlServer;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading.SqlServerMutex
{
    internal class SqlServerMutexFactory : IMutexFactory
    {
        private readonly string _connectionString;

        public SqlServerMutexFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IMutex Create(string key)
        {
            var @lock = new SqlDistributedLock(key, _connectionString);
            var sqlDistributedLockHandle = @lock.Acquire();
            return new SqlServerMutex(sqlDistributedLockHandle);
        }

        public async Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken)
        {
            var @lock = new SqlDistributedLock(key, _connectionString);
            var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
            return new SqlServerMutex(sqlDistributedLockHandle);
        }
    }
}
