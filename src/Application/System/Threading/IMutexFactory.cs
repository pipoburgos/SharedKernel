namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// Create mutex objects
    /// </summary>
    public interface IMutexFactory
    {
        /// <summary>
        /// Create a mutex object by a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IMutex Create(string key);

        /// <summary>
        /// Create a mutex object by a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IMutex> CreateAsync(string key, CancellationToken cancellationToken);
    }
}