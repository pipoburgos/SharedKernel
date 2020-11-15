using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System;
using System.Collections.Generic;
using SharedKernel.Domain.Specifications.Common;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Redis.Repositories
{
    public abstract class RedisRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        protected readonly IDistributedCache DistributedCache;
        protected readonly IBinarySerializer BinarySerializer;
        protected readonly string AggregateName;

        protected RedisRepository(
            IDistributedCache distributedCache,
            IBinarySerializer binarySerializer)
        {
            AggregateName = typeof(TAggregateRoot).Name;
            DistributedCache = distributedCache;
            BinarySerializer = binarySerializer;
        }

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            var bytes = DistributedCache.Get(AggregateName + key);

            if (bytes == default || bytes.Length == 0)
                return default;

            return BinarySerializer.Deserialize<TAggregateRoot>(bytes);
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public bool Any<TKey>(TKey key)
        {
            var bytes = DistributedCache.Get(AggregateName + key);

            return bytes != default && bytes.Length > 0;
        }

        public void Update(TAggregateRoot aggregate)
        {
            DistributedCache.Set(AggregateName + aggregate.Id, BinarySerializer.Serialize(aggregate));
        }

        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Update(aggregateRoot);
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            throw new NotImplementedException();
        }

        public void Remove(TAggregateRoot aggregate)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TAggregateRoot> aggregate)
        {
            throw new NotImplementedException();
        }

        public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public bool Any(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public int Rollback()
        {
            return 0;
        }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
