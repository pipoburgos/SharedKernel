using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Mongo.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class MongoRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMongoCollection<TAggregateRoot> MongoCollection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mongoSettings"></param>
        protected MongoRepository(IOptions<MongoSettings> mongoSettings)
        {
            MongoCollection = new MongoClient(mongoSettings.Value.ConnectionString)
                .GetDatabase(mongoSettings.Value.Database)
                .GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected BsonDocument ToBsonDocument(string json) => !string.IsNullOrWhiteSpace(json) ? BsonSerializer.Deserialize<BsonDocument>(json) : new BsonDocument();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected BsonArray ToBsonArray(string json) => !string.IsNullOrWhiteSpace(json) ? BsonSerializer.Deserialize<BsonArray>(json) : new BsonArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        public void Add(TAggregateRoot aggregate)
        {
            MongoCollection.InsertOne(aggregate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            MongoCollection.InsertMany(aggregates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TAggregateRoot GetById<TKey>(TKey key)
        {
            return MongoCollection.Find(a => a.Id.Equals(key)).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return MongoCollection.AsQueryable().Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Any<TKey>(TKey key)
        {
            return MongoCollection.Find(a => a.Id.Equals(key)).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TAggregateRoot entity)
        {
            MongoCollection.FindOneAndReplace(a => a.Id.Equals(entity.Id), entity);
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
        /// <param name="aggregate"></param>
        public void Remove(TAggregateRoot aggregate)
        {
            MongoCollection.DeleteOne(a => a.Id.Equals(aggregate.Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        public void RemoveRange(IEnumerable<TAggregateRoot> aggregate)
        {
            foreach (var aggregateRoot in aggregate)
            {
                Remove(aggregateRoot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().Where(spec.SatisfiedBy()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().Single(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().SingleOrDefault(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public bool Any(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().Any(spec.SatisfiedBy());
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
