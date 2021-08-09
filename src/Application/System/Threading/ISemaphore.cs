using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISemaphore
    {
        /// <summary>
        /// Create a SemaphoreSlim per key an block threads from given key
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> RunOneAtATimeFromGivenKey<T>(string key, Func<T> funcion);

        /// <summary>
        /// Create a SemaphoreSlim per key an block threads from given key
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> RunOneAtATimeFromGivenKey<T>(string key, Func<Task<T>> funcion, CancellationToken cancellationToken);
    }
}
