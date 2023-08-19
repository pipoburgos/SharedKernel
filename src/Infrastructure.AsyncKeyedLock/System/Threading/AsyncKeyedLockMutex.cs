using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.AsyncKeyedLock.System.Threading;

/// <summary>  </summary>
public class AsyncKeyedLockMutex : IMutex
{
    private readonly IDisposable _disposable;

    /// <summary>  </summary>
    public AsyncKeyedLockMutex(IDisposable disposable)
    {
        _disposable = disposable;
    }

    /// <summary>  </summary>
    public void Release()
    {
        _disposable.Dispose();
    }
}
