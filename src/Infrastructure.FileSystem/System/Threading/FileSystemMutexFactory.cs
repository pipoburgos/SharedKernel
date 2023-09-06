using Medallion.Threading.FileSystem;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.FileSystem.System.Threading;

internal class FileSystemMutexFactory : IMutexFactory
{
    private readonly DirectoryInfo _directoryInfo;

    public FileSystemMutexFactory(DirectoryInfo directoryInfo)
    {
        _directoryInfo = directoryInfo;
    }
    public IDisposable Create(string key)
    {
        var @lock = new FileDistributedLock(_directoryInfo, key);
        var sqlDistributedLockHandle = @lock.Acquire();
        return sqlDistributedLockHandle;
    }

    public async Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var @lock = new FileDistributedLock(_directoryInfo, key);
        var sqlDistributedLockHandle = await @lock.AcquireAsync(cancellationToken: cancellationToken);
        return sqlDistributedLockHandle;
    }
}
