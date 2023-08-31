using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories.Create;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Repositories.Update;
using SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data.Repositories;

/// <summary>  </summary>
public abstract class MongoRepositoryAsync<TAggregateRoot, TId> : MongoRepository<TAggregateRoot, TId>,
    ICreateRepositoryAsync<TAggregateRoot>,
    //IReadRepositoryAsync<TAggregateRoot, TId>,
    IUpdateRepositoryAsync<TAggregateRoot>,
    //IDeleteRepositoryAsync<TAggregateRoot>,
    //IReadSpecificationRepositoryAsync<TAggregateRoot>,
    ISaveRepositoryAsync where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
{
    /// <summary>  </summary>
    protected readonly MongoUnitOfWorkAsync MongoUnitOfWorkAsync;

    /// <summary>  </summary>
    protected MongoRepositoryAsync(MongoUnitOfWorkAsync mongoUnitOfWork) : base(mongoUnitOfWork)
    {
        MongoUnitOfWorkAsync = mongoUnitOfWork;
    }

    ///// <summary>  </summary>
    //public Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    //{
    //    return MongoCollection.Find(a => a.Id!.Equals(id))
    //        .SingleOrDefaultAsync<TAggregateRoot?>(cancellationToken: cancellationToken);
    //}

    ///// <summary>  </summary>
    //public Task<TAggregateRoot?> GetDeleteByIdAsync(TId id, CancellationToken cancellationToken)
    //{
    //    return MongoCollection.Find(a => a.Id!.Equals(id))
    //        .SingleOrDefaultAsync<TAggregateRoot?>(cancellationToken: cancellationToken);
    //}

    ///// <summary>  </summary>
    //public Task<bool> AnyAsync(CancellationToken cancellationToken)
    //{
    //    return MongoCollection.Find(FilterDefinition<TAggregateRoot>.Empty).AnyAsync(cancellationToken: cancellationToken);
    //}

    ///// <summary>  </summary>
    //public Task<bool> NotAnyAsync(CancellationToken cancellationToken)
    //{
    //    return Task.FromResult(NotAny());
    //}

    ///// <summary>  </summary>
    //public Task<long> CountAsync(CancellationToken cancellationToken)
    //{
    //    return MongoCollection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken);
    //}

    ///// <summary>  </summary>
    //public Task<bool> AnyAsync(TId id, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<bool> NotAnyAsync(TId id, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<bool> AnyAsync<TId1>(TId1 key, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<bool> NotAnyAsync<TId1>(TId1 key, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    /// <summary>  </summary>
    public Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.AddOperationAsync(() =>
            MongoCollection.InsertOneAsync(MongoUnitOfWork.GetSession(), aggregateRoot,
                cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.AddOperationAsync(() =>
            MongoCollection.InsertManyAsync(MongoUnitOfWork.GetSession(), aggregates,
                cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.AddOperationAsync(() =>
            MongoCollection.ReplaceOneAsync(MongoUnitOfWork.GetSession(), a => a.Id!.Equals(aggregateRoot.Id),
                aggregateRoot, cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates
            .Select(aggregateRoot =>
                MongoUnitOfWorkAsync.AddOperationAsync(() => UpdateAsync(aggregateRoot, cancellationToken))));
    }

    ///// <summary>  </summary>
    //public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregate, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<TAggregateRoot?> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    ///// <summary>  </summary>
    //public Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    /// <summary>  </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.SaveChangesResultAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.RollbackAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.RollbackResultAsync(cancellationToken);
    }
}
