using Microsoft.EntityFrameworkCore;
#if NET461 || NETSTANDARD2_1 || NETCOREAPP3_1
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using SharedKernel.Infrastructure.Data.Queryable;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using SharedKernel.Application.Adapter;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Paged;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries
{
    /// <summary>  </summary>
    public sealed class EntityFrameworkCoreQueryProvider<TDbContextBase> where TDbContextBase : DbContext, IDisposable
    {
        private readonly IDbContextFactory<TDbContextBase> _factory;
        private TDbContextBase _lastDbContext;
        private readonly List<TDbContextBase> _dbContexts;

        /// <summary>  </summary>
        public EntityFrameworkCoreQueryProvider(IDbContextFactory<TDbContextBase> factory)
        {
            _factory = factory;
            _dbContexts = new List<TDbContextBase>();
        }

        /// <summary>  </summary>
        public IQueryable<TEntity> GetQuery<TEntity>(bool showDeleted = false) where TEntity : class
        {
            _lastDbContext = GetDbContext();
            return Set<TEntity>(showDeleted);
        }

        /// <summary>  </summary>
        public IQueryable<TEntity> Set<TEntity>(bool showDeleted = false) where TEntity : class
        {
            if (_lastDbContext == default)
                throw new Exception("It is required to call the 'GetQuery' method before");

            return _lastDbContext
                .Set<TEntity>()
                .AsNoTracking()
                .Where(showDeleted, false);
        }

        private TDbContextBase GetDbContext()
        {
            var dbContext = _factory.CreateDbContext();
            _dbContexts.Add(dbContext);
            return dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageOptions"></param>
        /// <param name="domainSpecification"></param>
        /// <param name="dtoSpecification"></param>
        /// <param name="selector"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task<IPagedList<TResult>> ToPagedListAsync<T, TResult>(PageOptions pageOptions,
            ISpecification<T> domainSpecification = null, ISpecification<TResult> dtoSpecification = null,
            Expression<Func<T, TResult>> selector = null, CancellationToken cancellationToken = default)
            where T : class where TResult : class
        {
            var dbContext = _factory.CreateDbContext();
            _dbContexts.Add(dbContext);

            var query = dbContext.Set<T>().AsNoTracking();

            //var totalBefore = await query.CountAsync(cancellationToken);

            #region Domain Specifications

            if (pageOptions.ShowDeleted.HasValue && !pageOptions.ShowDeleted.Value && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(T)))
                query = query
                    .Cast<IEntityAuditableLogicalRemove>()
                    .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                    .Cast<T>();
            if (pageOptions.ShowDeleted.HasValue && pageOptions.ShowOnlyDeleted.HasValue && pageOptions.ShowDeleted.Value && pageOptions.ShowOnlyDeleted.Value &&
                typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(T)))
                query = query
                    .Cast<IEntityAuditableLogicalRemove>()
                    .Where(new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                    .Cast<T>();
            if (domainSpecification != null)
                query = query.Where(domainSpecification.SatisfiedBy());

            #endregion

            var queryDto = selector == default ? query.ProjectTo<TResult>() : query.Select(selector);

            #region Dto Specifications

            if (pageOptions.FilterProperties != null)
            {
                var propertiesSpec = new PropertiesContainsOrEqualSpecification<TResult>(
                    pageOptions.FilterProperties?.Select(p => new Property(p.Field, p.Value, default, true)));

                queryDto = queryDto.Where(propertiesSpec.SatisfiedBy());
            }

            if (!string.IsNullOrWhiteSpace(pageOptions.SearchText))
            {
                var searchTextSpec = new ObjectContainsOrEqualSpecification<TResult>(pageOptions.SearchText);

                queryDto = queryDto.Where(searchTextSpec.SatisfiedBy());
            }

            if (dtoSpecification != null)
                queryDto = queryDto.Where(dtoSpecification.SatisfiedBy());

            #endregion

            var totalAfter = await queryDto.CountAsync(cancellationToken);

            var elements = await queryDto
                .OrderAndPaged(pageOptions)
                .ToListAsync(cancellationToken);

            return new PagedList<TResult>(totalAfter, elements);
        }

        /// <summary>  </summary>
        public void Dispose()
        {
            foreach (var dbContextBase in _dbContexts)
            {
                dbContextBase.Dispose();
            }
        }
    }
}
