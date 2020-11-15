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
    public abstract class MongoRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        protected readonly IMongoCollection<TAggregateRoot> MongoCollection;

        protected MongoRepository(IOptions<MongoSettings> mongoSettings)
        {
            MongoCollection = new MongoClient(mongoSettings.Value.ConnectionString)
                .GetDatabase(mongoSettings.Value.Database)
                .GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
        }


        protected BsonDocument ToBsonDocument(string json) => !string.IsNullOrWhiteSpace(json) ? BsonSerializer.Deserialize<BsonDocument>(json) : new BsonDocument();

        protected BsonArray ToBsonArray(string json) => !string.IsNullOrWhiteSpace(json) ? BsonSerializer.Deserialize<BsonArray>(json) : new BsonArray();



        public void Add(TAggregateRoot aggregate)
        {
            MongoCollection.InsertOne(aggregate);
        }

        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            MongoCollection.InsertMany(aggregates);
        }

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            return MongoCollection.Find(a => a.Id.Equals(key)).SingleOrDefault();
        }

        public bool Any()
        {
            return MongoCollection.AsQueryable().Any();
        }

        public bool Any<TKey>(TKey key)
        {
            return MongoCollection.Find(a => a.Id.Equals(key)).Any();
        }

        public void Update(TAggregateRoot entity)
        {
            MongoCollection.FindOneAndReplace(a => a.Id.Equals(entity.Id), entity);
        }

        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Update(aggregateRoot);
            }
        }

        public void Remove(TAggregateRoot aggregate)
        {
            MongoCollection.DeleteOne(a => a.Id.Equals(aggregate.Id));
        }

        public void RemoveRange(IEnumerable<TAggregateRoot> aggregate)
        {
            foreach (var aggregateRoot in aggregate)
            {
                Remove(aggregateRoot);
            }
        }

        public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().Where(spec.SatisfiedBy()).ToList();
        }

        public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().Single(spec.SatisfiedBy());
        }

        public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().SingleOrDefault(spec.SatisfiedBy());
        }

        public bool Any(ISpecification<TAggregateRoot> spec)
        {
            return MongoCollection.AsQueryable().Any(spec.SatisfiedBy());
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
