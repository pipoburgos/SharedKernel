using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Reflection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Api.Grids.DataTables
{
    /// <summary>
    /// Command bus extensions
    /// </summary>
    public static class QueryBusExtensions
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
        public static async Task<DataTablesResponse<TResponse>> Ask<T, TResponse>(this IQueryBus queryBus,
            DataTablesRequest<T> request, CancellationToken cancellationToken) where T : IQueryRequest<IPagedList<TResponse>>
        {
            var showDeleted = ReflectionHelper.GetProperty<T, bool>(request.Filter, DeletedKey);

            var showOnlyDeleted = ReflectionHelper.GetProperty<T, bool>(request.Filter, OnlyDeletedKey);

            var columns = request.Columns.Where(c => c.Orderable).Select(c => c.Data).ToArray();

            var orders = request.Order.Select(c => new Order(columns[c.Column], c.Dir == "asc"));

            var pageOptions = new PageOptions(request.Start, request.Length, request.Search?.Value, showDeleted,
                showOnlyDeleted, orders, null);

            ReflectionHelper.SetProperty(request.Filter, nameof(PageOptions), pageOptions);

            var pagedList = await queryBus.Ask(request.Filter, cancellationToken);


            return new DataTablesResponse<TResponse>(request.Draw, pagedList);
        }

        /// <summary>
        /// Create query and send it over the command and query bus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="queryBus"></param>
        /// <param name="request"></param>
        /// <param name="fillOtherProperties"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public static async Task<DataTablesResponse<TResponse>> SendAsync<T, TResponse>(this IQueryBus queryBus,
            DataTablesRequest request, Action<T> fillOtherProperties = null, CancellationToken cancellationToken = default) where T : IQueryRequest<IPagedList<TResponse>>
        {
            var obj = ReflectionHelper.CreateInstance<T>();

            var showDeleted = request.AdditionalParameters.Any(ap => ap.Key == DeletedKey && ap.Value.Equals(true));

            var showOnlyDeleted = request.AdditionalParameters.Any(ap => ap.Key == OnlyDeletedKey && ap.Value.Equals(true));

            var columns = request.Columns.Where(c => c.Orderable).Select(c => c.Data).ToArray();

            var orders = request.Order.Select(c => new Order(columns[c.Column - 1], c.Dir == "asc"));

            var pageOptions = new PageOptions(request.Start, request.Length, request.Search?.Value, showDeleted, showOnlyDeleted, orders,
                null);

            ReflectionHelper.SetProperty(obj, nameof(PageOptions), pageOptions);

            foreach (var additionalParameter in request.AdditionalParameters.Where(ap => ap.Key != DeletedKey))
                ReflectionHelper.SetProperty(obj, additionalParameter.Key, additionalParameter.Value);

            fillOtherProperties?.Invoke(obj);

            var pagedList = await queryBus.Ask(obj, cancellationToken);

            return new DataTablesResponse<TResponse>(request.Draw, pagedList);
        }
    }
}
