using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.System;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Repositories
{
    /// <summary>
    ///     ENTITY FRAMEWORK CORE REPOSITORY
    /// </summary>
    /// <typeparam name="TAggregateRoot">Repository data type</typeparam>
    public abstract class EntityFrameworkCoreRepositoryAsync<TAggregateRoot> : EntityFrameworkCoreRepository<TAggregateRoot>, IRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextBase"></param>
        protected EntityFrameworkCoreRepositoryAsync(DbContextBase dbContextBase) : base(dbContextBase)
        {
        }

        #endregion Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery(true, true).Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<List<TAggregateRoot>> GetAllAsync(CancellationToken cancellationToken)
        {
            return GetQuery().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return GetQuery(false).AnyAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return DbContextBase.SetAggregate<TAggregateRoot>().CountAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery(false).Cast<IEntity<TKey>>().AnyAsync(a => a.Id.Equals(key), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false).Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery().SingleAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery().SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false).AnyAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            return DbContextBase.SetAggregate<TAggregateRoot>().AddAsync(aggregateRoot, cancellationToken).AsTask();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            return DbContextBase.SetAggregate<TAggregateRoot>().AddRangeAsync(aggregates, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().Remove(aggregateRoot);
            return TaskHelper.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().RemoveRange(aggregates);
            return TaskHelper.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().Update(aggregateRoot);
            return TaskHelper.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().UpdateRange(aggregates);
            return TaskHelper.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            return DbContextBase.RollbackAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return DbContextBase.SaveChangesAsync(cancellationToken);
        }
    }
}
