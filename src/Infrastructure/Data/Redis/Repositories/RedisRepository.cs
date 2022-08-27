using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System;
using System.Collections.Generic;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Redis.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class RedisRepository<TAggregateRoot, TKey> :
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>,
        IPersistRepository
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IDistributedCache DistributedCache;

        /// <summary>
        /// 
        /// </summary>
        protected readonly IBinarySerializer BinarySerializer;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string AggregateName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="binarySerializer"></param>
        protected RedisRepository(
            IDistributedCache distributedCache,
            IBinarySerializer binarySerializer)
        {
            AggregateName = typeof(TAggregateRoot).Name;
            DistributedCache = distributedCache;
            BinarySerializer = binarySerializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TAggregateRoot GetById<TKey>(TKey key)
        {
            var bytes = DistributedCache.Get(AggregateName + key);

            if (bytes == default || bytes.Length == 0)
                return default;

            return BinarySerializer.Deserialize<TAggregateRoot>(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool NotAny()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Any<TKey>(TKey key)
        {
            var bytes = DistributedCache.Get(AggregateName + key);

            return bytes != default && bytes.Length > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool NotAny<TKey1>(TKey1 key)
        {
            var bytes = DistributedCache.Get(AggregateName + key);

            return bytes == default || bytes.Length == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        public void Update(TAggregateRoot aggregate)
        {
            DistributedCache.Set(AggregateName + aggregate.Id, BinarySerializer.Serialize(aggregate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Update(aggregateRoot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 0;
        }
    }
}
