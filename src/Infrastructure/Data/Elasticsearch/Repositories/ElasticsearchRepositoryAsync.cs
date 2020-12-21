using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Data.Elasticsearch.Client;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.Elasticsearch.Repositories
{
    public abstract class ElasticsearchRepositoryAsync<TAggregateRoot> : ElasticsearchRepository<TAggregateRoot>,
        IPersistRepositoryAsync
        where TAggregateRoot : class, IAggregateRoot // , IEntity<TKey>
    {
        protected ElasticsearchRepositoryAsync(ElasticsearchClient client) : base(client)
        {
        }

        //public Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<TAggregateRoot> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<TAggregateRoot> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> AnyAsync(CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<int> CountAsync(CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken)
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
