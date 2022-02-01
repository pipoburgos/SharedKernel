using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using System;
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
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class EntityFrameworkCoreRepositoryAsync<TAggregateRoot, TDbContext> : EntityFrameworkCoreRepositoryAsync<TAggregateRoot>, IDisposable
        where TAggregateRoot : class, IAggregateRoot
        where TDbContext : DbContextBase
    {

        #region Members

        /// <summary>
        /// Db Context Factory for concurrence async await operations
        /// </summary>
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;

        private readonly List<TDbContext> _contexts;

        #endregion Members

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextBase"></param>
        /// <param name="dbContextFactory"></param>
        protected EntityFrameworkCoreRepositoryAsync(DbContextBase dbContextBase, IDbContextFactory<TDbContext> dbContextFactory) : base(dbContextBase)
        {
            _dbContextFactory = dbContextFactory;
            _contexts = new List<TDbContext>();
        }

        #endregion Constructors

        /// <summary>
        /// Creates a new DbContext an invoke Set method with no tracking
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected IQueryable<TEntity> SetNew<TEntity>() where TEntity : class
        {
            var context = _dbContextFactory.CreateDbContext();
            _contexts.Add(context);
            return context.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// Creates a new DbContext an get aggregate with no tracking
        /// </summary>
        /// <param name="showDeleted"></param>
        /// <returns></returns>
        protected IQueryable<TAggregateRoot> GetNewQuery(bool showDeleted = false)
        {
            var context = _dbContextFactory.CreateDbContext();
            _contexts.Add(context);
            return GetQuery(false, showDeleted, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> GetByIdNewContextAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetNewQuery()
                .Cast<IEntity<TKey>>()
                .Where(a => a.Id.Equals(key))
                .Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> GetDeleteByIdNewContextAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetNewQuery(true)
                .Cast<IEntity<TKey>>()
                .Where(a => a.Id.Equals(key))
                .Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<List<TAggregateRoot>> GetAllNewContextAsync(CancellationToken cancellationToken)
        {
            return GetNewQuery().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return GetNewQuery().AnyAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return GetNewQuery().CountAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetNewQuery()
                .Cast<IEntity<TKey>>()
                .AnyAsync(a => a.Id.Equals(key), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<List<TAggregateRoot>> WhereNewContextAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetNewQuery().Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TAggregateRoot> SingleNewContextAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetNewQuery().SingleAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TAggregateRoot> SingleOrDefaultNewContextAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetNewQuery().SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetNewQuery().AnyAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            foreach (var dbContextBase in _contexts)
            {
                dbContextBase.Dispose();
            }
        }
    }
}
