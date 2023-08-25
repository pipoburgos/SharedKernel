namespace SharedKernel.Application.Cqrs.Queries;

/// <summary> Query bus request abstaction. </summary>
public interface IQueryRequestHandler<in TRequest, TResponse> where TRequest : IQueryRequest<TResponse>
{
    /// <summary> Query handler. </summary>
    Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken);
}
