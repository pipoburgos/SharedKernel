using Microsoft.Extensions.Options;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Mongo.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class MongoRepositoryAsync<TAggregateRoot, TKey> : MongoRepository<TAggregateRoot, TKey>,
        ICreateRepositoryAsync<TAggregateRoot>,
        IPersistRepositoryAsync//, IRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mongoSettings"></param>
        protected MongoRepositoryAsync(IOptions<MongoSettings> mongoSettings) : base(mongoSettings)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            return MongoCollection.InsertOneAsync(aggregate, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            return MongoCollection.InsertManyAsync(aggregates, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
