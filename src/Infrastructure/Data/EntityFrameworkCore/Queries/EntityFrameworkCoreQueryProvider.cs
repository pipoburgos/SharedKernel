using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Cqrs.Queries.Entities;
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
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Extensions;

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

        /// <summary>  </summary>
        public QueryBuilder<TEntity> Set<TEntity>(PageOptions pageOptions, int numberOfConcurrentQueries = 3)
            where TEntity : class
        {
            return new QueryBuilder<TEntity>(GetDbContext(),
                numberOfConcurrentQueries < 2 ? default : GetDbContext(),
                numberOfConcurrentQueries < 3 ? default : GetDbContext(), pageOptions);
        }

        /// <summary>  </summary>
        public QueryBuilderDto<TEntity, TResult> Set<TEntity, TResult>(PageOptions pageOptions,
            int numberOfConcurrentQueries = 3) where TEntity : class
        {
            return new QueryBuilderDto<TEntity, TResult>(GetDbContext(),
                numberOfConcurrentQueries < 2 ? default : GetDbContext(),
                numberOfConcurrentQueries < 3 ? default : GetDbContext(), pageOptions);
        }

        /// <summary>  </summary>
        public Task<IPagedList<TResult>> ToPagedListAsync<T, TResult>(PageOptions pageOptions,
            ISpecification<T> domainSpecification = null, ISpecification<TResult> dtoSpecification = null,
            Expression<Func<T, TResult>> selector = null, CancellationToken cancellationToken = default)
            where T : class
        {
            return GetDbContext()
                .Set<T>()
                .AsNoTracking()
                .ToPagedListAsync(pageOptions, domainSpecification, dtoSpecification, selector, cancellationToken);
        }

        private TDbContextBase GetDbContext()
        {
            var dbContext = _factory.CreateDbContext();
            _dbContexts.Add(dbContext);
            return dbContext;
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
