using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.Dapper.Queries
{
    /// <summary> </summary>
    public class QueryResult
    {
        private readonly string _connectionString;

        /// <summary> </summary>
        public QueryResult(CustomDynamicParameters parameters, string query, string tempTables, string connectionString, PageOptions state)
        {
            _connectionString = connectionString;

            var parameterProperties = parameters.ParameterNames
                .Select(p => "@" + p)
                .ToList();

            var parametersNotFound = query
                .Replace("\n", " ")
                .Replace("\r", " ")
                .Replace("\t", " ")
                .Replace(")", " )")
                .Split(' ')
                .Where(word => word.StartsWith("@"))
                .Where(parameter => !parameterProperties.Contains(parameter))
                .Select(parameter => $"Parameter {parameter} not found")
                .ToList();

            if (parametersNotFound.Any())
                throw new Exception(string.Join(", ", parametersNotFound));

            Parameters = parameters;

            CountQuery = $"{tempTables} SELECT COUNT(1) FROM ({query}) ALIAS";

            PagedQuery = $"{tempTables} {query}";

            if (state.Orders != null && state.Orders.Any())
                PagedQuery += $"{Environment.NewLine} ORDER BY {string.Join(", ", state.Orders.Select(order => $"{order.Field} {(order.Ascending ? string.Empty : "DESC")}"))}";

            if (state.Take.HasValue)
                PagedQuery += $"{Environment.NewLine} OFFSET {state.Skip ?? 0} ROWS\nFETCH NEXT {state.Take} ROWS ONLY";
        }

        private CustomDynamicParameters Parameters { get; }

        private string CountQuery { get; }

        private string PagedQuery { get; }

        /// <summary> </summary>
        public async Task<IPagedList<TResult>> ToPagedListAsync<TResult>(CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync(cancellationToken);

            var itemsTask = connection.QueryAsync<TResult>(PagedQuery, Parameters);
            var countTask = connection.QueryFirstOrDefaultAsync<int>(CountQuery, Parameters);

            await Task.WhenAll(itemsTask, countTask);

            var items = await itemsTask;
            var count = await countTask;

            return new PagedList<TResult>(count, count, items.ToList());
        }
    }
}