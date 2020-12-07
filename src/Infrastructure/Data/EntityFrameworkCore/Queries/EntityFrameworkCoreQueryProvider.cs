using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Adapter;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Extensions;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Entities.Paged;
using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries
{
    public sealed class EntityFrameworkCoreQueryProvider<TDbContextBase> where TDbContextBase : DbContextBase
    {
        private readonly IDbContextFactory<TDbContextBase> _factory;

        public EntityFrameworkCoreQueryProvider(IDbContextFactory<TDbContextBase> factory)
        {
            _factory = factory;
        }

        public IQueryable<TEntity> GetQuery<TEntity>(bool showDeleted = false) where TEntity : class
        {
            var query = _factory.CreateDbContext().Set<TEntity>().AsNoTracking();

            if (!showDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(TEntity)))
            {
                query = query
                    .Cast<IEntityAuditableLogicalRemove>()
                    .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                    .Cast<TEntity>();
            }

            return query;
        }

        public async Task<IPagedList<TResult>> ToPagedListAsync<T, TResult>(PageOptions pageOptions,
            ISpecification<T> domainSpecification = null, ISpecification<TResult> dtoSpecification = null,
            Expression<Func<T, TResult>> selector = null, CancellationToken cancellationToken = default)
            where T : class where TResult : class
        {
            var query = _factory.CreateDbContext().Set<T>().AsNoTracking();

            var totalBefore = await query.CountAsync(cancellationToken);

            #region Domain Specifications

            if (!pageOptions.ShowDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(T)))
                query = query
                    .Cast<IEntityAuditableLogicalRemove>()
                    .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                    .Cast<T>();
            if (pageOptions.ShowDeleted && pageOptions.ShowOnlyDeleted && typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(T)))
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
                    pageOptions.FilterProperties?.Select(p => new Property(p.Field, p.Value)));

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


            return new PagedList<TResult>(totalBefore, totalAfter, elements);
        }
    }
}
