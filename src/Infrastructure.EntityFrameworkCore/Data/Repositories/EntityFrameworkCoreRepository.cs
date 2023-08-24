using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories
{
    /// <summary>
    ///     ENTITY FRAMEWORK CORE REPOSITORY
    /// </summary>
    /// <typeparam name="TAggregateRoot">Repository data type</typeparam>
    public abstract class EntityFrameworkCoreRepository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region Members

        /// <summary> Db context base </summary>
        protected readonly DbContextBase DbContextBase;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextBase"></param>
        protected EntityFrameworkCoreRepository(DbContextBase dbContextBase)
        {
            DbContextBase = dbContextBase ?? throw new ArgumentNullException(nameof(dbContextBase));
        }

        #endregion Constructors

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracking"></param>
        /// <param name="showDeleted"></param>
        /// <param name="dbContextBase"></param>
        /// <returns></returns>
        protected IQueryable<TAggregateRoot> GetQuery(bool tracking = true, bool showDeleted = false,
            DbContextBase? dbContextBase = default)
        {
            IQueryable<TAggregateRoot> query = (dbContextBase ?? DbContextBase).SetAggregate<TAggregateRoot>();

            query = GetAggregate(query);

            if (typeof(IEntityIsTranslatable<>).IsAssignableFrom(typeof(TAggregateRoot)))
            {
                query = query
                    .Cast<IEntityIsTranslatable<dynamic>>()
                    .Include(a => a.Translations)
                    .Cast<TAggregateRoot>();
            }

            if (!showDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(TAggregateRoot)))
            {
                query = query
                    .Cast<IEntityAuditableLogicalRemove>()
                    .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                    .Cast<TAggregateRoot>();
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (tracking)
                return query;

            return query.AsNoTracking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IQueryable<TAggregateRoot> GetAggregate(IQueryable<TAggregateRoot> query)
        {
            return query;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TAggregateRoot GetById<TKey>(TKey key)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id!.Equals(key)).Cast<TAggregateRoot>().SingleOrDefault()!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Any()
        {
            return GetQuery(false).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NotAny()
        {
            return !GetQuery(false).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Any<TKey>(TKey key)
        {
            return GetQuery(false).Cast<IEntity<TKey>>().Where(a => a.Id!.Equals(key)).Cast<TAggregateRoot>().Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool NotAny<TKey>(TKey key)
        {
            return !GetQuery(false).Cast<IEntity<TKey>>().Where(a => a.Id!.Equals(key)).Cast<TAggregateRoot>().Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public virtual List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().Where(spec.SatisfiedBy()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>

        public virtual TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().Single(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public virtual TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().SingleOrDefault(spec.SatisfiedBy())!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public virtual bool Any(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery(false).Any(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public bool NotAny(ISpecification<TAggregateRoot> spec)
        {
            return !GetQuery(false).Any(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public virtual void Add(TAggregateRoot aggregateRoot)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().Add(aggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoots"></param>
        public virtual void AddRange(IEnumerable<TAggregateRoot> aggregateRoots)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().AddRange(aggregateRoots);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public virtual void Remove(TAggregateRoot aggregateRoot)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().Remove(aggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public virtual void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().RemoveRange(aggregates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public virtual void Update(TAggregateRoot aggregateRoot)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().Update(aggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public virtual void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            DbContextBase.SetAggregate<TAggregateRoot>().UpdateRange(aggregates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return DbContextBase.Rollback();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return DbContextBase.SaveChanges();
        }
    }
}
