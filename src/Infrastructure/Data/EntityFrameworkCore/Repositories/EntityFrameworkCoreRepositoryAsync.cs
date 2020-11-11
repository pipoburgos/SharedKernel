using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Repositories
{
    /// <summary>
    ///     ENTITY FRAMEWORK BASE REPOSITORY
    /// </summary>
    /// <typeparam name="TAggregateRoot">Tipo de datos del repositorio</typeparam>
    public abstract class EntityFrameworkCoreRepositoryAsync<TAggregateRoot> : EntityFrameworkCoreRepository<TAggregateRoot>, IRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region Members

        private readonly IQueryableUnitOfWork _dbContextBase;

        private readonly DbSet<TAggregateRoot> _dbSet;

        #endregion Members

        #region Constructors

        protected EntityFrameworkCoreRepositoryAsync(DbContextBase dbContextBase) : base(dbContextBase)
        {
            _dbContextBase = dbContextBase ?? throw new ArgumentNullException(nameof(dbContextBase));
            _dbSet = dbContextBase.SetAggregate<TAggregateRoot>();
        }

        #endregion Constructors

        public Task<TAggregateRoot> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken);
        }

        public Task<TAggregateRoot> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery(true,true).Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken);
        }
        public Task<List<TAggregateRoot>> GetAllAsync(CancellationToken cancellationToken)
        {
            return GetQuery().ToListAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return GetQuery().AnyAsync(cancellationToken);
        }

        public Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery().Cast<IEntity<TKey>>().AnyAsync(a => a.Id.Equals(key), cancellationToken);
        }

        public Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false).Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
        }

        public Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery().SingleAsync(spec.SatisfiedBy(), cancellationToken);
        }

        public Task<TAggregateRoot> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery().SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken);
        }

        public Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false).AnyAsync(spec.SatisfiedBy(), cancellationToken);
        }

        public Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            return _dbSet.AddAsync(aggregateRoot, cancellationToken).AsTask();
        }

        public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken)
        {
            return _dbSet.AddRangeAsync(aggregateRoots, cancellationToken);
        }

        public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            _dbSet.Remove(aggregateRoot);
            return Task.FromResult(0);
        }

        public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            _dbSet.RemoveRange(aggregates);
            return Task.FromResult(0);
        }

        public Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            _dbSet.Update(aggregateRoot);
            return Task.FromResult(0);
        }

        public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            _dbSet.UpdateRange(aggregates);
            return Task.FromResult(0);
        }

        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            return _dbContextBase.RollbackAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dbContextBase.SaveChangesAsync(cancellationToken);
        }
    }
}
