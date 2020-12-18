using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Reflection;

namespace SharedKernel.Api.Grids.KendoGrid
{
    /// <summary>
    /// Command bus extensions
    /// </summary>
    public static class CommandBusExtensions
    {
        private const string DeletedKey = "Deleted";
        private const string OnlyDeletedKey = "OnlyDeleted";

        /// <summary>
        /// Create query and send it over the command and query bus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="queryBus"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public static async Task<KendoGridResponse<TResponse>> SendAsync<T, TResponse>(this IQueryBus queryBus,
            KendoGridRequest<T> request, CancellationToken cancellationToken) where T : IQueryRequest<IPagedList<TResponse>>
        {
            var showDeleted = ReflectionHelper.GetProperty<T, bool>(request.Filter, DeletedKey);

            var showOnlyDeleted = ReflectionHelper.GetProperty<T, bool>(request.Filter, OnlyDeletedKey);

            var orders = request.State.Sort.Select(a => new Order(a.Field, a.Dir == "asc"));

            var filterProperties = request.State.Filter?.Filters?.Select(f => new FilterProperty(f.Field, f.Value));

            var pageOptions = new PageOptions(request.State.Skip, request.State.Take, null, showDeleted, showOnlyDeleted, orders,
                filterProperties);

            ReflectionHelper.SetProperty(request.Filter, nameof(PageOptions), pageOptions);

            var pagedList = await queryBus.Ask(request.Filter, cancellationToken);

            return new KendoGridResponse<TResponse>(pagedList.Items, pagedList.TotalRecordsFiltered);
        }
    }
}
