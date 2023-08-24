namespace SharedKernel.Application.System.Threading;

/// <summary> Mutex manager. </summary>
public interface IMutexManager
{
    /// <summary> Run one at time from given key. </summary>
    void RunOneAtATimeFromGivenKey(string key, Action action);

    /// <summary> Run one at time from given key. </summary>
    T RunOneAtATimeFromGivenKey<T>(string key, Func<T> function);

#if !NET40
    /// <summary> Run one at time from given key. </summary>
    Task<T> RunOneAtATimeFromGivenKeyAsync<T>(string key, Func<Task<T>> function, CancellationToken cancellationToken);
#endif
}
