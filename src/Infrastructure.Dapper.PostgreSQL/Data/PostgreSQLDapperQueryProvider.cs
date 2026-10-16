
using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedKernel.Infrastructure.Dapper.PostgreSql.Data;

/// <summary>
/// Proveedor de consultas Dapper para PostgreSQL.
/// </summary>
public sealed class PostgreSqlDapperQueryProvider : IDapperQueryProvider
{
    private readonly ICustomLogger<PostgreSqlDapperQueryProvider> _logger;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    /// <summary>
    /// Inicializa el proveedor de consultas.
    /// </summary>
    public PostgreSqlDapperQueryProvider(
        ICustomLogger<PostgreSqlDapperQueryProvider> logger,
        IDbConnectionFactory dbConnectionFactory)
    {
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
    }

    /// <inheritdoc />
    public async Task<T?> ExecuteQueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = default,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            parameters,
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<T>(command);
    }

    /// <inheritdoc />
    public async Task<T> ExecuteQueryFirstAsync<T>(
        string sql,
        object? parameters = default,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            parameters,
            cancellationToken: cancellationToken);

        return await connection.QueryFirstAsync<T>(command);
    }

    /// <inheritdoc />
    public async Task<List<T>> ExecuteQueryAsync<T>(
        string sql,
        object? parameters = default,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            parameters,
            cancellationToken: cancellationToken);

        var result = await connection.QueryAsync<T>(command);

        return result.ToList();
    }

    /// <inheritdoc />
    public QueryBuilder Set(PageOptions pageOptions)
    {
        return new QueryBuilder(
            _dbConnectionFactory,
            pageOptions,
            true);
    }

    /// <inheritdoc />
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
                {
                    throw new ArgumentException(
                        "El campo del filtro es obligatorio.");
                }

                // Solo se permiten nombres de columna simples.
                // El campo debe existir en el resultado de la consulta.
                if (!Regex.IsMatch(
                    filter.Field,
                    @"^[a-zA-Z_][a-zA-Z0-9_$]*$"))
                {
                    throw new ArgumentException(
                        $"Nombre de campo no válido: {filter.Field}");
                }

                var field = $"filtered_query.\"{filter.Field}\"";
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
                        {
                            var pattern = op switch
                            {
                                FilterOperator.StartsWith =>
                                    $"{EscapeLike(value)}%",

                                FilterOperator.NotStartsWith =>
                                    $"{EscapeLike(value)}%",

                                FilterOperator.EndsWith =>
                                    $"%{EscapeLike(value)}",

                                FilterOperator.NotEndsWith =>
                                    $"%{EscapeLike(value)}",

                                FilterOperator.Contains =>
                                    $"%{EscapeLike(value)}%",

                                FilterOperator.NotContains =>
                                    $"%{EscapeLike(value)}%",

                                _ => throw new ArgumentOutOfRangeException(
                                    nameof(filter.Operator),
                                    op,
                                    @"Operador de filtrado no soportado."),
                            };

                            var textField = $"CAST({field} AS TEXT)";

                            if (filter.IgnoreCase != false)
                            {
                                textField = $"LOWER({textField})";
                                pattern = pattern.ToLowerInvariant();
                            }

                            var likeOperator = op is
                                FilterOperator.NotStartsWith or
                                FilterOperator.NotEndsWith or
                                FilterOperator.NotContains
                                    ? "NOT LIKE"
                                    : "LIKE";

                            conditions.Add(
                                $"{textField} {likeOperator} " +
                                $"@{parameterName} ESCAPE '\\'");

                            queryParameters.Add(parameterName, pattern);
                            break;
                        }

                    case FilterOperator.IsEmpty:
                        conditions.Add(
                            $"({field} IS NULL OR " +
                            $"CAST({field} AS TEXT) = '')");
                        break;

                    case FilterOperator.IsNotEmpty:
                        conditions.Add(
                            $"({field} IS NOT NULL AND " +
                            $"CAST({field} AS TEXT) <> '')");
                        break;

                    case FilterOperator.DateEqual:
                        conditions.Add(
                            $"CAST({field} AS DATE) = " +
                            $"CAST(@{parameterName} AS DATE)");

                        queryParameters.Add(parameterName, value);
                        break;

                    case FilterOperator.NotDateEqual:
                        conditions.Add(
                            $"({field} IS NULL OR " +
                            $"CAST({field} AS DATE) <> " +
                            $"CAST(@{parameterName} AS DATE))");

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
            ) AS filtered_query
            {whereSql}
            """;

        var countSql = $"""
            {pre}
            SELECT COUNT(1)
            FROM (
                {filteredSql}
            ) AS total_query;
            """;

        var querySql = new StringBuilder(
            $"{pre}{filteredSql}");

        if (pageOptions?.Orders?.Any() == true)
        {
            var orders = pageOptions.Orders.Select(order =>
            {
                if (string.IsNullOrWhiteSpace(order.Field))
                {
                    throw new ArgumentException(
                        "El campo de ordenación es obligatorio.");
                }

                if (!Regex.IsMatch(
                    order.Field,
                    @"^[a-zA-Z_][a-zA-Z0-9_$]*$"))
                {
                    throw new ArgumentException(
                        $"Campo de ordenación no válido: {order.Field}");
                }

                return $"filtered_query.\"{order.Field}\" " +
                    $"{(order.Ascending == false ? "DESC" : "ASC")}";
            });

            querySql.AppendLine(
                $" ORDER BY {string.Join(", ", orders)}");
        }
        else
        {
            querySql.AppendLine(" ORDER BY filtered_query.\"Id\"");
        }

        var skip = Math.Max(0, pageOptions?.Skip ?? 0);

        querySql.AppendLine(
            $" OFFSET {skip}");

        if (pageOptions?.Take is not null)
        {
            if (pageOptions.Take.Value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageOptions.Take));
            }

            querySql.AppendLine(
                $" LIMIT {pageOptions.Take.Value}");
        }

        var commandText =
            $"{countSql}{Environment.NewLine}{querySql}";

        _logger.Verbose(commandText);

        var command = new CommandDefinition(
            commandText,
            queryParameters,
            cancellationToken: cancellationToken);

        await using var results =
            await connection.QueryMultipleAsync(command);

        var total = await results.ReadSingleAsync<int>();
        var items = await results.ReadAsync<T>();

        return new PagedList<T>(total, items);
    }

    private static string EscapeLike(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> ExecuteSpMultipleAsync<T>(
        string storedProcedure,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await using var connection = _dbConnectionFactory.GetConnection();
#else
        using var connection = _dbConnectionFactory.GetConnection();
#endif
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var reader = await connection.QueryMultipleAsync(command);

        return await reader.ReadAsync<T>();
    }
}
