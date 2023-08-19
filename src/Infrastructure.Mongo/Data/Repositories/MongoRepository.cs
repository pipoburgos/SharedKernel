using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Infrastructure.Mongo.Data.Repositories;

/// <summary>  </summary>
public abstract class MongoRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
{
    /// <summary>  </summary>
    protected readonly IMongoCollection<TAggregateRoot> MongoCollection;

    /// <summary>  </summary>
    protected MongoRepository(IOptions<MongoSettings> mongoSettings)
    {
        MongoCollection = new MongoClient(mongoSettings.Value.ConnectionString)
            .GetDatabase(mongoSettings.Value.Database)
            .GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
    }

    /// <summary>  </summary>
    protected BsonDocument ToBsonDocument(string json) => !string.IsNullOrWhiteSpace(json)
        ? BsonSerializer.Deserialize<BsonDocument>(json)
        : new BsonDocument();

    /// <summary>  </summary>
    protected BsonArray ToBsonArray(string json) => !string.IsNullOrWhiteSpace(json)
        ? BsonSerializer.Deserialize<BsonArray>(json)
        : new BsonArray();

    /// <summary>  </summary>
    public void Add(TAggregateRoot aggregate)
    {
        MongoCollection.InsertOne(aggregate);
    }

    /// <summary>  </summary>
    public void AddRange(IEnumerable<TAggregateRoot> aggregates)
    {
        MongoCollection.InsertMany(aggregates);
    }

    /// <summary>  </summary>
    public TAggregateRoot GetById(TKey key)
    {
        return MongoCollection.Find(a => a.Id!.Equals(key)).SingleOrDefault();
    }

    /// <summary>  </summary>
    public bool Any()
    {
        return MongoCollection.AsQueryable().Any();
    }

    /// <summary>  </summary>
    public bool NotAny()
    {
        return !MongoCollection.AsQueryable().Any();
    }

    /// <summary>  </summary>
    public bool Any(TKey key)
    {
        return MongoCollection.Find(a => a.Id!.Equals(key)).Any();
    }

    /// <summary>  </summary>
    public bool NotAny(TKey key)
    {
        return !MongoCollection.Find(a => a.Id!.Equals(key)).Any();
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot entity)
    {
        MongoCollection.FindOneAndReplace(a => a.Id!.Equals(entity.Id), entity);
    }

    /// <summary>  </summary>
    public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public void Remove(TAggregateRoot aggregate)
    {
        MongoCollection.DeleteOne(a => a.Id!.Equals(aggregate.Id));
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregate)
    {
        foreach (var aggregateRoot in aggregate)
        {
            Remove(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
    {
        return MongoCollection.AsQueryable().Where(spec.SatisfiedBy()).ToList();
    }

    /// <summary>  </summary>
    public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
    {
        return MongoCollection.AsQueryable().Single(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
    {
        return MongoCollection.AsQueryable().SingleOrDefault(spec.SatisfiedBy()) ?? default!;
    }

    /// <summary>  </summary>
    public bool Any(ISpecification<TAggregateRoot> spec)
    {
        return MongoCollection.AsQueryable().Any(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public bool NotAny(ISpecification<TAggregateRoot> spec)
    {
        return !MongoCollection.AsQueryable().Any(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        return 0;
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        return 0;
    }
}
