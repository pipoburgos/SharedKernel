using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Redis.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class RedisRepositoryAsync<TAggregateRoot, TKey> : RedisRepository<TAggregateRoot, TKey>,
        IPersistRepositoryAsync
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="binarySerializer"></param>
        protected RedisRepositoryAsync(IDistributedCache distributedCache, IBinarySerializer binarySerializer) : base(distributedCache, binarySerializer) { }

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
