using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.Queryable;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Extensions
{
    /// <summary>  </summary>
    public static class QueryableExtensions
    {
        /// <summary>  </summary>
        public static async Task<IPagedList<TResult>> ToPagedListAsync<T, TResult>(this IQueryable<T> queryable,
            PageOptions pageOptions, ISpecification<T> domainSpecification = null,
            ISpecification<TResult> dtoSpecification = null, Expression<Func<T, TResult>> selector = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var query = queryable
                .Where(pageOptions.ShowDeleted, pageOptions.ShowOnlyDeleted)
                .Where(pageOptions.FilterProperties)
                .Where(domainSpecification?.SatisfiedBy() ?? new TrueSpecification<T>().SatisfiedBy())
                .MapToDto(selector)
                .Where(dtoSpecification?.SatisfiedBy() ?? new TrueSpecification<TResult>().SatisfiedBy())
                .Where(pageOptions.SearchText);

            var total = await query.CountAsync(cancellationToken);

            var elements = await query.OrderAndPaged(pageOptions).ToListAsync(cancellationToken);

            return new PagedList<TResult>(total, total, elements);
        }
    }
}
