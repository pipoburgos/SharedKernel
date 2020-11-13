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
    public abstract class RedisRepository<TAggregateRoot, TKey> :
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IBinarySerializer _binarySerializer;
        private readonly string _aggregateName;

        protected RedisRepository(
            IDistributedCache distributedCache,
            IBinarySerializer binarySerializer)
        {
            _aggregateName = typeof(TAggregateRoot).Name;
            _distributedCache = distributedCache;
            _binarySerializer = binarySerializer;
        }

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            var bytes = _distributedCache.Get(_aggregateName + key);

            if (bytes == default || bytes.Length == 0)
                return default;

            return _binarySerializer.Deserialize<TAggregateRoot>(bytes);
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
            _distributedCache.Set(_aggregateName + aggregate.Id, _binarySerializer.Serialize(aggregate));
        }

        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Update(aggregateRoot);
            }
        }
    }
}
