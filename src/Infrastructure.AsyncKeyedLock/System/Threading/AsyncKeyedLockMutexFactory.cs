using AsyncKeyedLock;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.AsyncKeyedLock.System.Threading;

internal class AsyncKeyedLockMutexFactory : IMutexFactory
{
    private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;

    /// <summary> Mutex manager constructor. </summary>
    public AsyncKeyedLockMutexFactory(AsyncKeyedLocker<string> asyncKeyedLocker)
    {
        _asyncKeyedLocker = asyncKeyedLocker;
    }

    public IMutex Create(string key)
    {
        return new AsyncKeyedLockMutex(_asyncKeyedLocker.Lock(key));
    }

    public async Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var disposable = await _asyncKeyedLocker.LockAsync(key, cancellationToken).ConfigureAwait(false);

        return new AsyncKeyedLockMutex(disposable);
    }
}
