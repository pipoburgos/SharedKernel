using SharedKernel.Application.Requests;
using SharedKernel.Domain.Requests;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public interface IExecuteMiddlewaresService
{
    /// <summary>  </summary>
    Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest;

    /// <summary>  </summary>
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>;
}
