using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using System.Data.Common;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

/// <summary> . </summary>
public sealed class DapperQueryProvider : IDisposable
{
    private readonly ICustomLogger<DapperQueryProvider> _logger;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly List<DbConnection> _connections;

    /// <summary> . </summary>
    public DapperQueryProvider(
        ICustomLogger<DapperQueryProvider> logger,
        IDbConnectionFactory dbConnectionFactory)
    {
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
        _connections = new List<DbConnection>();
    }

    /// <summary> . </summary>
    public async Task<T?> ExecuteQueryFirstOrDefaultAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default)
    {
        var connection = _dbConnectionFactory.GetConnection();
        _connections.Add(connection);
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    /// <summary> . </summary>
    public async Task<List<T>> ExecuteQueryAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default)
    {
        var connection = _dbConnectionFactory.GetConnection();
        _connections.Add(connection);
        await connection.OpenAsync(cancellationToken);
        return (await connection.QueryAsync<T>(sql, parameters)).ToList();
    }

    /// <summary> . </summary>
    public QueryBuilder Set(PageOptions pageOptions)
    {
        return new QueryBuilder(_dbConnectionFactory, pageOptions, true);
    }

    /// <summary> . </summary>
    public async Task<IPagedList<T>> ToPagedListSync<T>(string sql, object parameters, PageOptions pageOptions,
        string? preselect = default, CancellationToken cancellationToken = default)
    {
        var connection = _dbConnectionFactory.GetConnection();
        _connections.Add(connection);
        await connection.OpenAsync(cancellationToken);

        var pre = string.Empty;
        if (!string.IsNullOrWhiteSpace(preselect))
            pre = $"{preselect} {Environment.NewLine}";

        var queryCountString = $"{pre}SELECT COUNT(1) FROM ({sql}) ALIAS";

        _logger.Verbose(queryCountString);
        var total = await connection.QueryFirstOrDefaultAsync<int>(queryCountString, parameters);

        if (total == default)
            return PagedList<T>.Empty();

        var queryString = $"{preselect}{sql}";
        if (pageOptions.Orders != default! && pageOptions.Orders.Any())
        {
            var orders = pageOptions.Orders.Select(order =>
                order.Field + (!order.Ascending.HasValue || order.Ascending.Value ? string.Empty : " DESC"));
            queryString += $"{Environment.NewLine}ORDER BY {string.Join(", ", orders)}";
        }

        if (pageOptions.Take.HasValue)
            queryString += $"{Environment.NewLine}OFFSET {pageOptions.Skip} ROWS FETCH NEXT {pageOptions.Take} ROWS ONLY";

        _logger.Verbose(queryString);
        var elements = await connection.QueryAsync<T>(queryString, parameters);

        return new PagedList<T>(total, elements);
    }

    /// <summary> . </summary>
    public async Task<IPagedList<T>> ToPagedListAsync<T>(string sql, object parameters, PageOptions pageOptions,
        string? preselect = default, CancellationToken cancellationToken = default)
    {
        var connection = _dbConnectionFactory.GetConnection();
        _connections.Add(connection);
        await connection.OpenAsync(cancellationToken);

        var pre = string.Empty;
        if (!string.IsNullOrWhiteSpace(preselect))
            pre = $"{preselect} {Environment.NewLine}";

        var queryCountString = $"{pre}SELECT COUNT(1) FROM ({sql}) ALIAS";

        _logger.Verbose(queryCountString);
        var totalTask = connection.QueryFirstOrDefaultAsync<int>(queryCountString, parameters);

        var queryString = $"{preselect}{sql}";
        if (pageOptions.Orders != default! && pageOptions.Orders.Any())
        {
            var orders = pageOptions.Orders.Select(order =>
                order.Field + (!order.Ascending.HasValue || order.Ascending.Value ? string.Empty : " DESC"));
            queryString += $"{Environment.NewLine}ORDER BY {string.Join(", ", orders)}";
        }

        if (pageOptions.Take.HasValue)
            queryString += $"{Environment.NewLine}OFFSET {pageOptions.Skip} ROWS FETCH NEXT {pageOptions.Take} ROWS ONLY";

        _logger.Verbose(queryString);
        var elementsTask = connection.QueryAsync<T>(queryString, parameters);


        await Task.WhenAll(totalTask, elementsTask);
        return new PagedList<T>(await totalTask, await elementsTask);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        foreach (var dbConnection in _connections)
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }
    }

    /// <summary> . </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary> . </summary>
    ~DapperQueryProvider()
    {
        Dispose(false);
    }
}
