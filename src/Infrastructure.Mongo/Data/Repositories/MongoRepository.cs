using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data.Repositories;

/// <summary>  </summary>
public abstract class MongoRepository<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId>, ISaveRepository
    where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
{
    /// <summary>  </summary>
    protected readonly MongoUnitOfWork MongoUnitOfWork;

    /// <summary>  </summary>
    protected readonly IMongoCollection<TAggregateRoot> MongoCollection;

    /// <summary>  </summary>
    protected MongoRepository(MongoUnitOfWork mongoUnitOfWork)
    {
        MongoUnitOfWork = mongoUnitOfWork;
        MongoCollection = mongoUnitOfWork.GetCollection<TAggregateRoot>();
    }

    #region Protected Methods

    /// <summary>  </summary>
    protected IQueryable<TAggregateRoot> GetQuery(bool showDeleted = false)
    {
        IQueryable<TAggregateRoot> query = MongoCollection.AsQueryable();

        query = GetAggregate(query);

        if (!showDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(TAggregateRoot)))
        {
            query = query
                .Cast<IEntityAuditableLogicalRemove>()
                .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                .Cast<TAggregateRoot>();
        }

        return query;
    }

    /// <summary>  </summary>
    protected virtual IQueryable<TAggregateRoot> GetAggregate(IQueryable<TAggregateRoot> query)
    {
        return query;
    }

    #endregion

    /// <summary>  </summary>
    protected BsonDocument ToBsonDocument(string json) => !string.IsNullOrWhiteSpace(json)
        ? BsonSerializer.Deserialize<BsonDocument>(json)
        : new BsonDocument();

    /// <summary>  </summary>
    protected BsonArray ToBsonArray(string json) => !string.IsNullOrWhiteSpace(json)
        ? BsonSerializer.Deserialize<BsonArray>(json)
        : new BsonArray();

    /// <summary>  </summary>
    public void Add(TAggregateRoot aggregateRoot)
    {
        MongoUnitOfWork.AddOperation(() => MongoCollection.InsertOne(MongoUnitOfWork.GetSession(), aggregateRoot), aggregateRoot);
    }

    /// <summary>  </summary>
    public void AddRange(IEnumerable<TAggregateRoot> aggregates)
    {
        MongoUnitOfWork.AddOperation(() => MongoCollection.InsertMany(MongoUnitOfWork.GetSession(), aggregates), aggregates);
    }

    /// <summary>  </summary>
    public TAggregateRoot? GetById(TId id)
    {
        return MongoCollection.Find(a => a.Id!.Equals(id)).SingleOrDefault();
    }

    /// <summary>  </summary>
    public bool Any()
    {
        return MongoCollection.Find(FilterDefinition<TAggregateRoot>.Empty).Any();
    }

    /// <summary>  </summary>
    public bool NotAny()
    {
        return !GetQuery().Any();
    }

    /// <summary>  </summary>
    public bool Any(TId id)
    {
        return MongoCollection.Find(a => a.Id!.Equals(id)).Any();
    }

    /// <summary>  </summary>
    public bool NotAny(TId id)
    {
        return !MongoCollection.Find(a => a.Id!.Equals(id)).Any();
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot aggregateRoot)
    {
        MongoUnitOfWork.UpdateOperation(() =>
            MongoCollection.FindOneAndReplace(MongoUnitOfWork.GetSession(), a => a.Id!.Equals(aggregateRoot.Id),
                aggregateRoot), aggregateRoot);
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
    public void Remove(TAggregateRoot aggregateRoot)
    {
        MongoUnitOfWork.RemoveOperation(() =>
            MongoCollection.DeleteOne(MongoUnitOfWork.GetSession(), a => a.Id!.Equals(aggregateRoot.Id)), aggregateRoot);
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregate)
    {
        MongoUnitOfWork.RemoveOperation(() =>
            MongoCollection.DeleteMany(MongoUnitOfWork.GetSession(),
                a => aggregate.Select(x => x.Id).Contains(a.Id)), aggregate);
    }

    /// <summary>  </summary>
    public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Where(spec.SatisfiedBy()).ToList();
    }

    /// <summary>  </summary>
    public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Single(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public TAggregateRoot? SingleOrDefault(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().SingleOrDefault(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public bool Any(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Any(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public bool NotAny(ISpecification<TAggregateRoot> spec)
    {
        return !GetQuery().Any(spec.SatisfiedBy());
    }

    /// <summary>  </summary>
    public Result<int> SaveChangesResult()
    {
        return MongoUnitOfWork.SaveChangesResult();
    }

    /// <summary>  </summary>
    public int Rollback()
    {
        return MongoUnitOfWork.Rollback();
    }

    /// <summary>  </summary>
    public Result<int> RollbackResult()
    {
        return MongoUnitOfWork.RollbackResult();
    }

    /// <summary>  </summary>
    public int SaveChanges()
    {
        return MongoUnitOfWork.SaveChanges();
    }
}
