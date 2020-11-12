using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Caching;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Caching
{
    internal class DistributedCacheHelper : ICacheHelper
    {
        private readonly IDistributedCache _distributedCache;
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        public DistributedCacheHelper(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null)
        {
            try
            {

                await Semaphore.WaitAsync();
                var value = await _distributedCache.GetAsync(key);

                if (value != default && value.Length != 0)
                    return ByteArrayToObject<T>(value);


                var valueToCache = await generator();
                await _distributedCache.SetAsync(key, ObjectToByteArray(valueToCache),
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

        /// <summary>
        /// Convert an Object to byte array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Convert a byte array to an Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        private T ByteArrayToObject<T>(byte[] arrBytes)
        {
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (T)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
