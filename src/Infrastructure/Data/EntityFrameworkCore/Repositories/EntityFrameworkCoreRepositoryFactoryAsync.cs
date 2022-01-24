using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
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
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class EntityFrameworkCoreRepositoryFactoryAsync<TAggregateRoot, TDbContext> : EntityFrameworkCoreRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
        where TDbContext : DbContextBase
    {

        #region Members

        /// <summary>
        /// Db Context Factory for concurrence async await operations
        /// </summary>
        protected readonly IDbContextFactory<TDbContext> DbContextFactory;

        #endregion Members

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextBase"></param>
        /// <param name="dbContextFactory"></param>
        protected EntityFrameworkCoreRepositoryFactoryAsync(DbContextBase dbContextBase, IDbContextFactory<TDbContext> dbContextFactory) : base(dbContextBase)
        {
            DbContextFactory = dbContextFactory;
        }

        #endregion Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot> GetByIdNewContextAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return GetQuery(false, false, DbContextFactory.CreateDbContext())
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
            return GetQuery(false, true, DbContextFactory.CreateDbContext())
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
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).AnyAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).CountAsync(cancellationToken);
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
            return GetQuery(false, false, DbContextFactory.CreateDbContext())
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
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TAggregateRoot> SingleNewContextAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).SingleAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TAggregateRoot> SingleOrDefaultNewContextAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetQuery(false, false, DbContextFactory.CreateDbContext()).AnyAsync(spec.SatisfiedBy(), cancellationToken);
        }
    }
}
