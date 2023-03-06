using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Reflection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Api.Grids.KendoGrid
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
        public static async Task<KendoGridResponse<TResponse>> Ask<T, TResponse>(this IQueryBus queryBus,
            KendoGridRequest<T> request, CancellationToken cancellationToken) where T : IQueryRequest<IPagedList<TResponse>>
        {
            var showDeleted = ReflectionHelper.GetProperty<T, bool>(request.Filter, DeletedKey);

            var showOnlyDeleted = ReflectionHelper.GetProperty<T, bool>(request.Filter, OnlyDeletedKey);

            var orders = request.State.Sort.Select(a => new Order(a.Field, a.Dir == "asc"));

            var filterProperties = request.State.Filter?.Filters?.Select(f =>
                new FilterProperty(f.Field, f.Value, CastToFilterOperator(f.Operator), f.IgnoreCase));

            var pageOptions = new PageOptions(request.State.Skip, request.State.Take, null, showDeleted, showOnlyDeleted, orders,
                filterProperties);

            ReflectionHelper.SetProperty(request.Filter, nameof(PageOptions), pageOptions);

            var pagedList = await queryBus.Ask(request.Filter, cancellationToken);

            return new KendoGridResponse<TResponse>(pagedList.Items, pagedList.TotalRecordsFiltered);
        }

        private static FilterOperator? CastToFilterOperator(string @operator)
        {
            return @operator switch
            {
                "eq" => FilterOperator.EqualTo,
                "neq" => FilterOperator.NotEqualTo,
                "isnull" => FilterOperator.IsEqualToNull,
                "isnotnull" => FilterOperator.IsNotEqualToNull,
                "lt" => FilterOperator.LessThan,
                "lte" => FilterOperator.LessThanOrEqualTo,
                "gt" => FilterOperator.GreaterThan,
                "gte" => FilterOperator.GreaterThanOrEqualTo,
                "startswith" => FilterOperator.StartsWith,
                "endswith" => FilterOperator.EndsWith,
                "contains" => FilterOperator.Contains,
                "doesnotcontain" => FilterOperator.NotContains,
                "isempty" => FilterOperator.IsEmpty,
                "isnotempty" => FilterOperator.IsNotEmpty,
                "dateeq" => FilterOperator.DateEqual,
                "dateneq" => FilterOperator.NotDateEqual,
                _ => default
            };
        }
    }
}
