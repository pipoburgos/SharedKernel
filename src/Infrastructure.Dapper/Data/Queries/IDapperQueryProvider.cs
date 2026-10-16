using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

public interface IDapperQueryProvider
{
    /// <summary> . </summary>
    Task<T?> ExecuteQueryFirstOrDefaultAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default);

    Task<T> ExecuteQueryFirstAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<List<T>> ExecuteQueryAsync<T>(string sql, object? parameters = default,
        CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    QueryBuilder Set(PageOptions pageOptions);

    Task<IPagedList<T>> ToPagedListAsync<T>(
        string sql,
        object parameters,
        PageOptions? pageOptions,
        string? preselect = null,
        CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<IEnumerable<T>> ExecuteSpMultipleAsync<T>(string storedProcedure, object? parameters = null,
        CancellationToken cancellationToken = default);
}