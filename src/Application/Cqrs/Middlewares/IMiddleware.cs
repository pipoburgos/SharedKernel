namespace SharedKernel.Application.Cqrs.Middlewares;

/// <summary> Middleware that runs both on the command bus, as well as on the query and event bus. </summary>
public interface IMiddleware
{
    /// <summary> Middleware that runs both on the command bus, as well as on the query and event bus. </summary>
    Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next) where TRequest : IRequest;

    /// <summary> Middleware that runs both on the command bus, as well as on the query and event bus. </summary>
    Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next)
        where TRequest : IRequest<TResponse>;

    /// <summary> Middleware that runs both on the command bus, as well as on the query and event bus. </summary>
    Task<Result<TResponse>> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<Result<TResponse>>> next)
        where TRequest : IRequest<Result<TResponse>>;
}
