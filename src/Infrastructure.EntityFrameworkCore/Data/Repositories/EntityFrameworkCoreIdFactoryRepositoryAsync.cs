using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories
{
    /// <summary>
    ///     ENTITY FRAMEWORK CORE REPOSITORY
    /// </summary>
    /// <typeparam name="TAggregateRoot">Repository data type</typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public abstract class EntityFrameworkCoreRepositoryAsync<TAggregateRoot, TId, TDbContext> :
        EntityFrameworkCoreRepositoryAsync<TAggregateRoot, TId>,
        IReadOnlyRepositoryAsync<TAggregateRoot, TId>,
        IReadOnlySpecificationRepositoryAsync<TAggregateRoot>,
        IDisposable where TAggregateRoot : class, IAggregateRoot where TDbContext : DbContextBase
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
        protected EntityFrameworkCoreRepositoryAsync(TDbContext dbContextBase,
            IDbContextFactory<TDbContext> dbContextFactory) : base(dbContextBase)
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
        protected IQueryable<TEntity> SetReadOnly<TEntity>() where TEntity : class
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
        protected IQueryable<TAggregateRoot> GetReadOnlyQuery(bool showDeleted = false)
        {
            var context = _dbContextFactory.CreateDbContext();
            _contexts.Add(context);
            return GetQuery(false, showDeleted, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot?> GetByIdReadOnlyAsync(TId key, CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery()
                .Cast<IEntity<TId>>()
                .Where(a => a.Id!.Equals(key))
                .Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public virtual Task<TAggregateRoot?> GetDeleteByIdReadOnlyAsync(TId key,
            CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery(true)
                .Cast<IEntity<TId>>()
                .Where(a => a.Id!.Equals(key))
                .Cast<TAggregateRoot>()
                .SingleOrDefaultAsync(cancellationToken)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<List<TAggregateRoot>> GetAllReadOnlyAsync(CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().AnyAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().CountAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(TId key, CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery()
                .Cast<IEntity<TId>>()
                .AnyAsync(a => a.Id!.Equals(key), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<List<TAggregateRoot>> WhereReadOnlyAsync(ISpecification<TAggregateRoot> spec,
            CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().Where(spec.SatisfiedBy()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TAggregateRoot> SingleReadOnlyAsync(ISpecification<TAggregateRoot> spec,
            CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().SingleAsync(spec.SatisfiedBy(), cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TAggregateRoot?> SingleOrDefaultReadOnlyAsync(ISpecification<TAggregateRoot> spec,
            CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().SingleOrDefaultAsync(spec.SatisfiedBy(), cancellationToken)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public override Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            return GetReadOnlyQuery().AnyAsync(spec.SatisfiedBy(), cancellationToken);
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
