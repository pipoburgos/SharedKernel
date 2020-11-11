using Newtonsoft.Json;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedKernel.Infrastructure.Exceptions;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Redis.Repositories
{
    public abstract class RedisRepository<TAggregateRoot, TKey> :
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        protected RedisRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            var data = _database.StringGet(key.ToString());

            return data.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<TAggregateRoot>(data);
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public bool Any<TKey>(TKey key)
        {
            return _database.KeyExists(key.ToString());
        }

        public void Update(TAggregateRoot aggregate)
        {
            var created = _database.StringSet(aggregate.Id.ToString(), JsonConvert.SerializeObject(aggregate));

            if (!created)
            {
                throw new SharedKernelInfrastructureException(nameof(ExceptionCodes.REDIS_UPDATE));
            }
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
