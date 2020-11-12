using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Redis.Repositories
{
    public abstract class RedisRepository<TAggregateRoot, TKey> :
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly string _aggregateName;

        protected RedisRepository(IDistributedCache distributedCache)
        {
            _aggregateName = typeof(TAggregateRoot).Name;
            _distributedCache = distributedCache;
        }

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            var bytes = _distributedCache.Get(_aggregateName + key);

            if (bytes == default || bytes.Length == 0)
                return default;

            return ByteArrayToObject<TAggregateRoot>(bytes);
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public bool Any<TKey>(TKey key)
        {
            var bytes = _distributedCache.Get(_aggregateName + key);

            return bytes != default && bytes.Length > 0;
        }

        public void Update(TAggregateRoot aggregate)
        {
            _distributedCache.Set(_aggregateName + aggregate.Id, ObjectToByteArray(aggregate));
        }

        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Update(aggregateRoot);
            }
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
