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

    public IDisposable Create(string key)
    {
        return _asyncKeyedLocker.Lock(key);
    }

    public async Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken)
    {
        var disposable = await _asyncKeyedLocker.LockAsync(key, cancellationToken).ConfigureAwait(false);

        return disposable;
    }
}
