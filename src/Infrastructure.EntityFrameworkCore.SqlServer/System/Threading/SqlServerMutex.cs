using Medallion.Threading.SqlServer;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.System.Threading;

/// <summary> Sql Server mutex. </summary>
public class SqlServerMutex : IMutex
{
    private readonly SqlDistributedLockHandle _sqlDistributedLockHandle;

    /// <summary> Constructor. </summary>
    /// <param name="sqlDistributedLockHandle"></param>
    public SqlServerMutex(SqlDistributedLockHandle sqlDistributedLockHandle)
    {
        _sqlDistributedLockHandle = sqlDistributedLockHandle;
    }

    /// <summary> Release Sql Server mutex. </summary>
    public void Dispose()
    {
        _sqlDistributedLockHandle.Dispose();
    }
}
