using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.System.Threading;

namespace SharedKernel.Infrastructure.System.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomSemaphore : ISemaphore
    {
        private readonly ISemaphoreStore _semaphoreStore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semaphoreStore"></param>
        public CustomSemaphore(ISemaphoreStore semaphoreStore)
        {
            _semaphoreStore = semaphoreStore;
        }

        /// <summary>
        /// Create a SemaphoreSlim per key an block threads from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public async Task<T> RunOneAtATimeFromGivenKey<T>(string key, Func<T> funcion)
        {
            var semaforo = _semaphoreStore.GetOrCreate(key);
            try
            {
                await semaforo.WaitAsync();
                return funcion();
            }
            finally
            {
                semaforo.Release();
            }
        }

        /// <summary>
        /// Create a SemaphoreSlim per key an block threads from given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="funcion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> RunOneAtATimeFromGivenKey<T>(string key, Func<Task<T>> funcion, CancellationToken cancellationToken)
        {
            var semaforo = _semaphoreStore.GetOrCreate(key);
            try
            {
                await semaforo.WaitAsync(cancellationToken);
                return await funcion();
            }
            finally
            {
                semaforo.Release();
            }
        }
    }
}
