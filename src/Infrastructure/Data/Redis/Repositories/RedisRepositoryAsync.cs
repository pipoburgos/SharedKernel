using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.Redis.Repositories
{
    public abstract class RedisRepositoryAsync<TAggregateRoot, TKey> : RedisRepository<TAggregateRoot, TKey>,
        IPersistRepositoryAsync
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        protected RedisRepositoryAsync(IDistributedCache distributedCache, IBinarySerializer binarySerializer) : base(distributedCache, binarySerializer) { }

        //public Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<TAggregateRoot> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        //{
        //    var bytes = await DistributedCache.GetAsync(AggregateName + key, cancellationToken);

        //    if (bytes == default || bytes.Length == 0)
        //        return default;

        //    return BinarySerializer.Deserialize<TAggregateRoot>(bytes);
        //}

        //public Task<TAggregateRoot> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> AnyAsync(CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> AnyAsync<TKey1>(TKey1 key, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateAsync(TAggregateRoot entity, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task RemoveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregate, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<TAggregateRoot> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
