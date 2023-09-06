using Medallion.Threading.FileSystem;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.FileSystem.System.Threading;

/// <summary> Redis mutex. </summary>
internal class FileSystemMutex : IMutex
{
    private readonly FileDistributedLockHandle _redisDistributedLockHandle;

    /// <summary> Constructor. </summary>
    /// <param name="redisDistributedLockHandle"></param>
    public FileSystemMutex(FileDistributedLockHandle redisDistributedLockHandle)
    {
        _redisDistributedLockHandle = redisDistributedLockHandle;
    }

    /// <summary> Release Sql Server mutex. </summary>
    public void Dispose()
    {
        _redisDistributedLockHandle.Dispose();
    }
}