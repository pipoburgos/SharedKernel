namespace SharedKernel.Application.System.Threading;

/// <summary> Create mutex objects. </summary>
public interface IMutexFactory
{
    /// <summary> Create a mutex object by a key. </summary>
    IDisposable Create(string key);

    /// <summary> Create a mutex object by a key. </summary>
    Task<IDisposable> CreateAsync(string key, CancellationToken cancellationToken);
}
