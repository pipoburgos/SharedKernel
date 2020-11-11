using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Mongo.Repositories
{
    public abstract class MongoRepository<TAggregateRoot, TKey> :
        ICreateRepository<TAggregateRoot>,
        IReadRepository<TAggregateRoot>,
        IUpdateRepository<TAggregateRoot>,
        IDeleteRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        protected readonly IMongoCollection<TAggregateRoot> MongoCollection;

        protected MongoRepository(MongoClient mongoClient, string database)
        {
            MongoCollection = mongoClient.GetDatabase(database).GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
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
    }
}
