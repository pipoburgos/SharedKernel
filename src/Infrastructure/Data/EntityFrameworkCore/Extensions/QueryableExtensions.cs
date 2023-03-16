using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Cqrs.Queries.Kendo;
using SharedKernel.Infrastructure.Data.Queryable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Extensions
{
    /// <summary>  </summary>
    public static class QueryableExtensions
    {
        /// <summary>  </summary>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable,
            DataStateChange dataStateChange, CancellationToken cancellationToken)
        {
            return queryable.ToPagedListAsync(dataStateChange.ToPageOptions(), cancellationToken);
        }

        /// <summary>  </summary>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable,
            PageOptions pageOptions, CancellationToken cancellationToken)
        {
            var query = queryable
                .Where(pageOptions.FilterProperties)
                .Where(pageOptions.SearchText);

            var total = await query.CountAsync(cancellationToken);

            if (total == default)
                return PagedList<T>.Empty();

            var elements = await query.OrderAndPaged(pageOptions).ToListAsync(cancellationToken);

            return new PagedList<T>(total, elements);
        }
    }
}
