using MongoDB.Driver;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data.Repositories;

/// <summary>  </summary>
public abstract class MongoRepositoryAsync<TAggregateRoot, TId> : MongoRepository<TAggregateRoot, TId>,
    IRepositoryAsync<TAggregateRoot, TId>,
    ISaveRepositoryAsync where TAggregateRoot : class, IAggregateRoot, IEntity<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected readonly MongoUnitOfWorkAsync MongoUnitOfWorkAsync;

    /// <summary>  </summary>
    protected MongoRepositoryAsync(MongoUnitOfWorkAsync mongoUnitOfWork) : base(mongoUnitOfWork)
    {
        MongoUnitOfWorkAsync = mongoUnitOfWork;
    }

    /// <summary>  </summary>
    public Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.AddOperationAsync(aggregateRoot, () =>
            MongoCollection.InsertOneAsync(MongoUnitOfWork.GetSession(), aggregateRoot,
                cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        var list = aggregates.ToList();
        return MongoUnitOfWorkAsync.AddOperationAsync(list, () =>
            MongoCollection.InsertManyAsync(MongoUnitOfWork.GetSession(), list,
                cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var cursor = await MongoCollection.FindAsync(a => a.Id!.Equals(id), cancellationToken: cancellationToken);

        var aggregateRoot = await cursor.SingleOrDefaultAsync<TAggregateRoot?>(cancellationToken: cancellationToken);

        if (aggregateRoot is IEntityAuditableLogicalRemove ag)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(ag) ? default : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    public async Task<bool> AnyAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) != default;
    }

    /// <summary>  </summary>
    public async Task<bool> NotAnyAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) == default;
    }

    /// <summary>  </summary>
    public Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return MongoUnitOfWorkAsync.UpdateOperationAsync(aggregateRoot, () =>
            MongoCollection.ReplaceOneAsync(MongoUnitOfWork.GetSession(), a => a.Id!.Equals(aggregateRoot.Id),
                aggregateRoot, cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(aggregateRoot =>
            MongoUnitOfWorkAsync.AddOperationAsync(aggregateRoot,
                () => UpdateAsync(aggregateRoot, cancellationToken))));
    }

    /// <summary>  </summary>
    public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return MongoUnitOfWork.RemoveOperationAsync(aggregateRoot,
            () => MongoCollection.DeleteOneAsync(MongoUnitOfWork.GetSession(), a => a.Id!.Equals(aggregateRoot.Id),
                cancellationToken: cancellationToken),
            () => MongoCollection.ReplaceOneAsync(MongoUnitOfWork.GetSession(), b => b.Id!.Equals(aggregateRoot.Id),
                aggregateRoot, cancellationToken: cancellationToken));
    }

    /// <summary>  </summary>
    public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(aggregateRoot =>
            MongoUnitOfWorkAsync.AddOperationAsync(aggregateRoot,
                () => RemoveAsync(aggregateRoot, cancellationToken))));
    }

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
