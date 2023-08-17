using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Requests;
using SharedKernel.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public class ExecuteMiddlewaresService : IExecuteMiddlewaresService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>  </summary>
    public ExecuteMiddlewaresService(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>  </summary>
    public Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest
    {
        var middlewares = _serviceScopeFactory.CreateScope().ServiceProvider.GetServices<IMiddleware<TRequest>>().ToList();
        return !middlewares.Any()
            ? last(request, cancellationToken)
            : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
    }

    /// <summary>  </summary>
    public Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>
    {
        var middlewares = _serviceScopeFactory.CreateScope().ServiceProvider.GetServices<IMiddleware<TRequest, TResponse>>().ToList();
        return !middlewares.Any()
            ? last(request, cancellationToken)
            : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
    }

    private Func<TRequest, CancellationToken, Task> GetNext<TRequest>(IList<IMiddleware<TRequest>> middlewares,
        int i, int lastIndex, Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest
    {
        if (i == lastIndex)
            return last;

        i++;
        return (r, c) => middlewares[i - 1].Handle(r, c, GetNext(middlewares, i, lastIndex, last));
    }

    private Func<TRequest, CancellationToken, Task<TResponse>> GetNext<TRequest, TResponse>(IList<IMiddleware<TRequest, TResponse>> middlewares,
        int i, int lastIndex, Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>
    {
        if (i == lastIndex)
            return last;

        i++;
        return (r, c) => middlewares[i - 1].Handle(r, c, GetNext(middlewares, i, lastIndex, last));
    }
}
