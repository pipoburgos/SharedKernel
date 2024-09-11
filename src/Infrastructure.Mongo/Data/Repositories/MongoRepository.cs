using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories.Read;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Mongo.Data.DbContexts;

namespace SharedKernel.Infrastructure.Mongo.Data.Repositories;

/// <summary> . </summary>
public abstract class MongoRepository<TAggregateRoot, TId> : RepositoryAsync<TAggregateRoot, TId>,
    IReadAllRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary> . </summary>
    protected readonly MongoDbContext MongoUnitOfWork;


    /// <summary> . </summary>
    protected MongoRepository(MongoDbContext mongoDbContext) : base(mongoDbContext)
    {
        MongoUnitOfWork = mongoDbContext;
    }

    #region Protected Methods

    /// <summary> . </summary>
    protected IQueryable<TAggregateRoot> GetQuery(bool showDeleted = false)
    {
        IQueryable<TAggregateRoot> query = MongoUnitOfWork.Set<TAggregateRoot>().AsQueryable();

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

    /// <summary> . </summary>
    protected virtual IQueryable<TAggregateRoot> GetAggregate(IQueryable<TAggregateRoot> query)
    {
        return query;
    }

    #endregion

    /// <summary> . </summary>
    protected BsonDocument ToBsonDocument(string json) => !string.IsNullOrWhiteSpace(json)
        ? BsonSerializer.Deserialize<BsonDocument>(json)
        : new BsonDocument();

    /// <summary> . </summary>
    protected BsonArray ToBsonArray(string json) => !string.IsNullOrWhiteSpace(json)
        ? BsonSerializer.Deserialize<BsonArray>(json)
        : new BsonArray();

    /// <summary> . </summary>
    public List<TAggregateRoot> GetAll()
    {
        return MongoUnitOfWork.Set<TAggregateRoot>().Find(FilterDefinition<TAggregateRoot>.Empty).ToList();
    }

    /// <summary> . </summary>
    public bool Any()
    {
        return MongoUnitOfWork.Set<TAggregateRoot>().Find(FilterDefinition<TAggregateRoot>.Empty).Any();
    }

    /// <summary> . </summary>
    public bool NotAny()
    {
        return !GetQuery().Any();
    }

    /// <summary> . </summary>
    public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Where(spec.SatisfiedBy()).ToList();
    }

    /// <summary> . </summary>
    public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Single(spec.SatisfiedBy());
    }

    /// <summary> . </summary>
    public TAggregateRoot? SingleOrDefault(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().SingleOrDefault(spec.SatisfiedBy());
    }

    /// <summary> . </summary>
    public bool Any(ISpecification<TAggregateRoot> spec)
    {
        return GetQuery().Any(spec.SatisfiedBy());
    }

    /// <summary> . </summary>
    public bool NotAny(ISpecification<TAggregateRoot> spec)
    {
        return !GetQuery().Any(spec.SatisfiedBy());
    }
}
