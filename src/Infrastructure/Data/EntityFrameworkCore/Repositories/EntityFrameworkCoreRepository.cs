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
    ///     ENTITY FRAMEWORK BASE REPOSITORY
    /// </summary>
    /// <typeparam name="TAggregateRoot">Tipo de datos del repositorio</typeparam>
    public abstract class EntityFrameworkCoreRepository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region Members

        private readonly DbContextBase _dbContextBase;

        private readonly DbSet<TAggregateRoot> _dbSet;

        #endregion Members

        #region Constructors

        protected EntityFrameworkCoreRepository(DbContextBase dbContextBase)
        {
            _dbContextBase = dbContextBase ?? throw new ArgumentNullException(nameof(dbContextBase));
            _dbSet = dbContextBase.SetAggregate<TAggregateRoot>();
        }

        #endregion Constructors

        #region Protected Methods

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


        protected virtual IQueryable<TAggregateRoot> GetAggregate(IQueryable<TAggregateRoot> query)
        {
            return query;
        }

        #endregion

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>().SingleOrDefault();
        }

        public bool Any()
        {
            return GetQuery().Any();
        }

        public bool Any<TKey>(TKey key)
        {
            return GetQuery().Cast<IEntity<TKey>>().Where(a => a.Id.Equals(key)).Cast<TAggregateRoot>().Any();
        }

        public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery(false).Where(spec.SatisfiedBy()).ToList();
        }

        public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().Single(spec.SatisfiedBy());
        }

        public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery().SingleOrDefault(spec.SatisfiedBy());
        }

        public bool Any(ISpecification<TAggregateRoot> spec)
        {
            return GetQuery(false).Any(spec.SatisfiedBy());
        }

        public void Add(TAggregateRoot aggregateRoot)
        {
            _dbSet.Add(aggregateRoot);
        }

        public void AddRange(IEnumerable<TAggregateRoot> aggregateRoots)
        {
            _dbSet.AddRange(aggregateRoots);
        }

        public void Remove(TAggregateRoot aggregateRoot)
        {
            _dbSet.Remove(aggregateRoot);
        }

        public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
        {
            _dbSet.RemoveRange(aggregates);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            _dbSet.Update(aggregateRoot);
        }

        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            _dbSet.UpdateRange(aggregates);
        }

        public int Rollback()
        {
            return _dbContextBase.Rollback();
        }

        public int SaveChanges()
        {
            return _dbContextBase.SaveChanges();
        }
    }
}
