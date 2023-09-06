namespace SharedKernel.Application.System.Threading;

/// <summary> Mutex manager. </summary>
public class MutexManager : IMutexManager
{
    private readonly IMutexFactory _lockerFactory;

    /// <summary> Mutex manager constructor. </summary>
    public MutexManager(IMutexFactory lockerFactory)
    {
        _lockerFactory = lockerFactory;
    }

    /// <summary> Run one at time from given key. </summary>
    public void RunOneAtATimeFromGivenKey(string key, Action action)
    {
        var locker = _lockerFactory.Create(key);
        try
        {
            action();
        }
        finally
        {
            locker.Dispose();
        }
    }

    /// <summary> Run one at time from given key. </summary>
    public T RunOneAtATimeFromGivenKey<T>(string key, Func<T> function)
    {
        var locker = _lockerFactory.Create(key);
        try
        {
            return function();
        }
        finally
        {
            locker.Dispose();
        }
    }

#if !NET40
    /// <summary> Run one at time from given key. </summary>
    public async Task<T> RunOneAtATimeFromGivenKeyAsync<T>(string key, Func<Task<T>> function,
        CancellationToken cancellationToken)
    {
        var locker = await _lockerFactory.CreateAsync(key, cancellationToken);
        try
        {
            return await function();
        }
        finally
        {
            locker.Dispose();
        }
    }
#endif

}
