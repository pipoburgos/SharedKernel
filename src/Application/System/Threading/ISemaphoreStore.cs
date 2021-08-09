using System.Threading;

namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// Create and manage SemaphoreSlim by dictionary keys
    /// </summary>
    public interface ISemaphoreStore
    {
        /// <summary>
        /// Gets o create a SemaphoreSlim per key (Thread Safe)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        SemaphoreSlim GetOrCreate(string key);
    }
}
