using Microsoft.EntityFrameworkCore;
#if NET461 || NETSTANDARD2_1 || NETCOREAPP3_1
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using SharedKernel.Infrastructure.Data.Queryable;

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
