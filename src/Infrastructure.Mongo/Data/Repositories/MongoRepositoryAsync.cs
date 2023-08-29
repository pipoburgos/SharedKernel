using Microsoft.Extensions.Options;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

namespace SharedKernel.Infrastructure.Mongo.Data.Repositories;

/// <summary>  </summary>
public abstract class MongoRepositoryAsync<TAggregateRoot, TId> : MongoRepository<TAggregateRoot, TId>,
    ICreateRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
{
    /// <summary>  </summary>
    protected MongoRepositoryAsync(IOptions<MongoSettings> mongoSettings) : base(mongoSettings)
    {
    }

    /// <summary>  </summary>
    public Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
    {
        return MongoCollection.InsertOneAsync(aggregate, cancellationToken: cancellationToken);
    }

    /// <summary>  </summary>
    public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return MongoCollection.InsertManyAsync(aggregates, cancellationToken: cancellationToken);
    }
}
