﻿namespace SharedKernel.Application.Cqrs.Middlewares;

/// <summary>  </summary>
public interface IPipeline
{
    /// <summary>  </summary>
    Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest;

    /// <summary>  </summary>
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>;

    /// <summary>  </summary>
    Task<Result<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<Result<TResponse>>> last)
        where TRequest : IRequest<Result<TResponse>>;
}
