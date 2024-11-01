using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

/// <summary> </summary>
public class QueryResult
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    /// <summary> </summary>
    public QueryResult(DynamicParameters parameters, string query, string tempTables, PageOptions state, IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;

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

        if (state.Orders != default! && state.Orders.Any())
            PagedQuery += $"{Environment.NewLine} ORDER BY {string.Join(", ", state.Orders.Select(order => $"{order.Field} {(!order.Ascending.HasValue || order.Ascending.Value ? string.Empty : "DESC")}"))}";

        if (state.Take.HasValue)
            PagedQuery += $"{Environment.NewLine} OFFSET {state.Skip ?? 0} ROWS\nFETCH NEXT {state.Take} ROWS ONLY";
    }

    private DynamicParameters Parameters { get; }

    private string CountQuery { get; }

    private string PagedQuery { get; }

    /// <summary> </summary>
    public async Task<IPagedList<TResult>> ToPagedListAsync<TResult>(CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        var total = await connection.QueryFirstOrDefaultAsync<int>(CountQuery, Parameters);

        if (total == default)
            return PagedList<TResult>.Empty();

        var elements = await connection.QueryAsync<TResult>(PagedQuery, Parameters);

        return new PagedList<TResult>(total, elements);
    }
}