using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Caching
{
    internal class DistributedCacheHelper : ICacheHelper
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IBinarySerializer _binarySerializer;
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        public DistributedCacheHelper(
            IDistributedCache distributedCache,
            IBinarySerializer binarySerializer)
        {
            _distributedCache = distributedCache;
            _binarySerializer = binarySerializer;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null)
        {
            try
            {

                await Semaphore.WaitAsync();
                var value = await _distributedCache.GetAsync(key);

                if (value != default && value.Length != 0)
                    return _binarySerializer.Deserialize<T>(value);


                var valueToCache = await generator();
                await _distributedCache.SetAsync(key, _binarySerializer.Serialize(valueToCache),
                    new DistributedCacheEntryOptions());

                return valueToCache;
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
