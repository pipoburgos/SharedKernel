using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Repositories
{
    /// <summary>
    ///     ENTITY FRAMEWORK CORE REPOSITORY
    /// </summary>
    /// <typeparam name="TAggregateRoot">Repository data type</typeparam>
    public abstract class EntityFrameworkCoreRepository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region Members

        private readonly DbContextBase _dbContextBase;

        private readonly DbSet<TAggregateRoot> _dbSet;

        #endregion Members

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextBase"></param>
        protected EntityFrameworkCoreRepository(DbContextBase dbContextBase)
        {
            _dbContextBase = dbContextBase ?? throw new ArgumentNullException(nameof(dbContextBase));
            _dbSet = dbContextBase.SetAggregate<TAggregateRoot>();
        }

        #endregion Constructors

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracking"></param>
        /// <param name="showDeleted"></param>
        /// <returns></returns>
        protected IQueryable<TAggregateRoot> GetQuery(bool tracking = true, bool showDeleted = false)
        {
            IQueryable<TAggregateRoot> query = _dbSet;

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
        public TAggregateRoot GetById<TKey>(TKey key)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>().SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return GetQuery().Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Any<TKey>(TKey key)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>().Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery(false).Where(spec.SatisfiedBy()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>

        public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().Single(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().SingleOrDefault(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public bool Any(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery(false).Any(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public void Add(TAggregateRoot aggregateRoot)
        {
            _dbSet.Add(aggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoots"></param>
        public void AddRange(IEnumerable<TAggregateRoot> aggregateRoots)
        {
            _dbSet.AddRange(aggregateRoots);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public void Remove(TAggregateRoot aggregateRoot)
        {
            _dbSet.Remove(aggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
        {
            _dbSet.RemoveRange(aggregates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public void Update(TAggregateRoot aggregateRoot)
        {
            _dbSet.Update(aggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            _dbSet.UpdateRange(aggregates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return _dbContextBase.Rollback();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _dbContextBase.SaveChanges();
        }
    }
}
