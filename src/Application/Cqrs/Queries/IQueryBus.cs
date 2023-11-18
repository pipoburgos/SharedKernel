namespace SharedKernel.Application.Cqrs.Queries;

/// <summary> Query bus abstraction. </summary>
public interface IQueryBus
{
    /// <summary> Ask a query and return a data transfer object. </summary>
    Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query, CancellationToken cancellationToken);

    /// <summary> Ask a query and return a data transfer object. </summary>
    Task<Result<TResponse>> Ask<TResponse>(IQueryRequest<Result<TResponse>> query,
        CancellationToken cancellationToken);
}
