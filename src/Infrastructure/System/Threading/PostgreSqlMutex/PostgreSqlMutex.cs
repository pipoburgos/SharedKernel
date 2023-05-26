using Medallion.Threading.Postgres;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading.PostgreSqlMutex
{
    /// <summary>
    /// Sql Server mutex
    /// </summary>
    public class PostgreSqlMutex : IMutex
    {
        private readonly PostgresDistributedLockHandle _postgresDistributedLockHandle;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="postgresDistributedLockHandle"></param>
        public PostgreSqlMutex(PostgresDistributedLockHandle postgresDistributedLockHandle)
        {
            _postgresDistributedLockHandle = postgresDistributedLockHandle;
        }

        /// <summary>
        /// Release Sql Server mutex
        /// </summary>
        public void Release()
        {
            _postgresDistributedLockHandle.Dispose();
        }
    }
}