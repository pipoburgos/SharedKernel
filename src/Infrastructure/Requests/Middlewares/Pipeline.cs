using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public class Pipeline : IPipeline
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>  </summary>
    public Pipeline(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>  </summary>
    public Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest
    {
        var middlewares = GetMiddlewares();
        return !middlewares.Any()
            ? last(request, cancellationToken)
            : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
    }

    /// <summary>  </summary>
    public Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>
    {
        var middlewares = GetMiddlewares();
        return !middlewares.Any()
            ? last(request, cancellationToken)
            : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
    }

    /// <summary>  </summary>
    public Task<Result<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<Result<TResponse>>> last)
        where TRequest : IRequest<Result<TResponse>>
    {
        var middlewares = GetMiddlewares();
        return !middlewares.Any()
            ? last(request, cancellationToken)
            : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
    }

    private List<IMiddleware> GetMiddlewares()
    {
        return _serviceScopeFactory.CreateScope().ServiceProvider.GetServices<IMiddleware>().ToList();
    }

    private Func<TRequest, CancellationToken, Task> GetNext<TRequest>(IList<IMiddleware> middlewares,
        int i, int lastIndex, Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest
    {
        if (i == lastIndex)
            return last;

        i++;
        return (r, c) => middlewares[i - 1].Handle(r, c, GetNext(middlewares, i, lastIndex, last));
    }

    private Func<TRequest, CancellationToken, Task<TResponse>> GetNext<TRequest, TResponse>(
        IList<IMiddleware> middlewares, int i, int lastIndex, Func<TRequest, CancellationToken, Task<TResponse>> last)
        where TRequest : IRequest<TResponse>
    {
        if (i == lastIndex)
            return last;

        i++;
        return (r, c) => middlewares[i - 1].Handle(r, c, GetNext(middlewares, i, lastIndex, last));
    }

    private Func<TRequest, CancellationToken, Task<Result<TResponse>>> GetNext<TRequest, TResponse>(
        IList<IMiddleware> middlewares, int i, int lastIndex,
        Func<TRequest, CancellationToken, Task<Result<TResponse>>> last)
        where TRequest : IRequest<Result<TResponse>>
    {
        if (i == lastIndex)
            return last;

        i++;
        return (r, c) => middlewares[i - 1]
            .Handle<TRequest, Result<TResponse>>(r, c, GetNext(middlewares, i, lastIndex, last));
    }
}
