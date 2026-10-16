using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedKernel.Infrastructure.Dapper.SqlServer.Data;

/// <summary> . </summary>
public sealed class SqlServerDapperQueryProvider : IDapperQueryProvider
{
    private readonly ICustomLogger<SqlServerDapperQueryProvider> _logger;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    /// <summary> . </summary>
    public SqlServerDapperQueryProvider(
        ICustomLogger<SqlServerDapperQueryProvider> logger,
        IDbConnectionFactory dbConnectionFactory)
    {
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
    }

    /// <summary> . </summary>
    public async Task<T?> ExecuteQueryFirstOrDefaultAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public async Task<T> ExecuteQueryFirstAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryFirstAsync<T>(sql, parameters);
    }

    /// <summary> . </summary>
    public async Task<List<T>> ExecuteQueryAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);
        return (await connection.QueryAsync<T>(sql, parameters)).ToList();
    }

    /// <summary> . </summary>
    public QueryBuilder Set(PageOptions pageOptions)
    {
        return new QueryBuilder(_dbConnectionFactory, pageOptions, true);
    }

    public async Task<IPagedList<T>> ToPagedListAsync<T>(
        string sql,
        object parameters,
        PageOptions? pageOptions,
        string? preselect = null,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif

        await connection.OpenAsync(cancellationToken);

        var pre = string.IsNullOrWhiteSpace(preselect)
            ? string.Empty
            : $"{preselect}{Environment.NewLine}";

        var queryParameters = new DynamicParameters(parameters);
        var conditions = new List<string>();
        var filterIndex = 0;

        if (pageOptions?.FilterProperties is not null)
        {
            foreach (var filter in pageOptions.FilterProperties)
            {
                if (string.IsNullOrWhiteSpace(filter.Field))
                    throw new ArgumentException("El campo del filtro es obligatorio.");

                // El campo debe ser un alias o columna del SELECT original.
                if (!Regex.IsMatch(filter.Field, @"^[a-zA-Z_][a-zA-Z0-9_$]*$"))
                    throw new ArgumentException(
                        $"Nombre de campo no válido: {filter.Field}");

                var field = $"FilteredQuery.[{filter.Field}]";
                var value = filter.Value;
                var op = filter.Operator ?? FilterOperator.EqualTo;
                var parameterName = $"filter{filterIndex++}";

                switch (op)
                {
                    case FilterOperator.EqualTo:
                        conditions.Add($"{field} = @{parameterName}");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.NotEqualTo:
                        conditions.Add($"{field} <> @{parameterName}");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.IsEqualToNull:
                        conditions.Add($"{field} IS NULL");
                        break;

                    case FilterOperator.IsNotEqualToNull:
                        conditions.Add($"{field} IS NOT NULL");
                        break;

                    case FilterOperator.LessThan:
                        conditions.Add($"{field} < @{parameterName}");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.LessThanOrEqualTo:
                        conditions.Add($"{field} <= @{parameterName}");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.GreaterThan:
                        conditions.Add($"{field} > @{parameterName}");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.GreaterThanOrEqualTo:
                        conditions.Add($"{field} >= @{parameterName}");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.StartsWith:
                    case FilterOperator.NotStartsWith:
                    case FilterOperator.EndsWith:
                    case FilterOperator.NotEndsWith:
                    case FilterOperator.Contains:
                    case FilterOperator.NotContains:
                        var pattern = op switch
                        {
                            FilterOperator.StartsWith => $"{EscapeLike(value)}%",
                            FilterOperator.NotStartsWith => $"{EscapeLike(value)}%",
                            FilterOperator.EndsWith => $"%{EscapeLike(value)}",
                            FilterOperator.NotEndsWith => $"%{EscapeLike(value)}",
                            FilterOperator.Contains => $"%{EscapeLike(value)}%",
                            FilterOperator.NotContains => $"%{EscapeLike(value)}%",
                            _ => throw new ArgumentOutOfRangeException(nameof(filter.Operator), op, @"Operador de filtrado no soportado."),
                        };

                        var textField =
                            $"CONVERT(nvarchar(max), {field})";

                        if (filter.IgnoreCase != false)
                            textField = $"LOWER({textField})";

                        if (filter.IgnoreCase != false)
                            pattern = pattern.ToLowerInvariant();

                        var likeOperator = op is
                            FilterOperator.NotStartsWith or
                            FilterOperator.NotEndsWith or
                            FilterOperator.NotContains
                                ? "NOT LIKE"
                                : "LIKE";

                        conditions.Add(
                            $"{textField} {likeOperator} @{parameterName} ESCAPE '\\'");

                        queryParameters.Add(parameterName, pattern);
                        break;

                    case FilterOperator.IsEmpty:
                        conditions.Add(
                            $"({field} IS NULL OR " +
                            $"CONVERT(nvarchar(max), {field}) = '')");
                        break;

                    case FilterOperator.IsNotEmpty:
                        conditions.Add(
                            $"({field} IS NOT NULL AND " +
                            $"CONVERT(nvarchar(max), {field}) <> '')");
                        break;

                    case FilterOperator.DateEqual:
                        conditions.Add(
                            $"TRY_CONVERT(date, {field}) = " +
                            $"TRY_CONVERT(date, @{parameterName})");
                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.NotDateEqual:
                        conditions.Add(
                            $"({field} IS NULL OR " +
                            $"TRY_CONVERT(date, {field}) <> " +
                            $"TRY_CONVERT(date, @{parameterName}))");
                        queryParameters.Add(parameterName, value);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(filter.Operator),
                            op,
                            @"Operador de filtrado no soportado.");
                }
            }
        }

        var whereSql = conditions.Count > 0
            ? $"WHERE {string.Join(" AND ", conditions)}"
            : string.Empty;

        var filteredSql = $"""
        SELECT *
        FROM (
            {sql}
        ) AS FilteredQuery
        {whereSql}
        """;

        var countSql = $"""
        {pre}
        SELECT COUNT(1)
        FROM (
            {filteredSql}
        ) AS TotalQuery;
        """;

        var querySql = new StringBuilder(
            $"{pre}{filteredSql}");

        if (pageOptions?.Orders?.Any() == true)
        {
            var orders = pageOptions.Orders.Select(order =>
            {
                if (string.IsNullOrWhiteSpace(order.Field))
                    return string.Empty;

                if (!Regex.IsMatch(order.Field, @"^[a-zA-Z_][a-zA-Z0-9_$]*$"))
                {
                    throw new ArgumentException(
                        $"Campo de ordenación no válido: {order.Field}");
                }

                return $"FilteredQuery.[{order.Field}] " +
                       $"{(order.Ascending == false ? "DESC" : "ASC")}";
            });

            querySql.AppendLine(
                $" ORDER BY {string.Join(", ", orders)}");
        }
        else
        {
            querySql.AppendLine(" ORDER BY [Id]");
        }

        querySql.AppendLine(
            $" OFFSET {Math.Max(0, pageOptions?.Skip ?? 0)} ROWS");

        if (pageOptions?.Take is not null)
        {
            if (pageOptions.Take.Value < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(pageOptions.Take));

            querySql.AppendLine(
                $" FETCH NEXT {pageOptions.Take.Value} ROWS ONLY");
        }

        var commandText = $"{countSql}{Environment.NewLine}{querySql}";

        _logger.Verbose(commandText);

        var command = new CommandDefinition(commandText, queryParameters, cancellationToken: cancellationToken);

        await using var results = await connection.QueryMultipleAsync(command);

        var total = await results.ReadSingleAsync<int>();
        var items = await results.ReadAsync<T>();

        return new PagedList<T>(total, items);
    }

    private static string EscapeLike(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_")
            .Replace("[", "\\[");
    }


    /// <summary> . </summary>
    public async Task<IEnumerable<T>> ExecuteSpMultipleAsync<T>(string storedProcedure, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);

        var reader = await connection.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        return await reader.ReadAsync<T>();
    }
}