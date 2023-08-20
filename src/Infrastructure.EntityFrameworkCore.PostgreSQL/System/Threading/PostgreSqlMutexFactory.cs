using Medallion.Threading.Postgres;
using SharedKernel.Application.System.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.System.Threading
{
    internal class PostgreSqlMutexFactory : IMutexFactory
    {
        private readonly string _connectionString;

        public PostgreSqlMutexFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IMutex Create(string key)
        {
            var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(key), _connectionString);
            var sqlDistributedLockHandle = @lock.Acquire();
            return new PostgreSqlMutex(sqlDistributedLockHandle);
        }

        public async Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken)
        {
            var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(key, true), _connectionString);
            var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
            return new PostgreSqlMutex(sqlDistributedLockHandle);
        }
    }
}
