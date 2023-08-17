using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Failover;

internal class FailoverMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly FailoverCommonLogic _failoverCommonLogic;

    public FailoverMiddleware(FailoverCommonLogic failoverCommonLogic)
    {
        _failoverCommonLogic = failoverCommonLogic;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next)
    {
        try
        {
            return await next(request, cancellationToken);
        }
        catch (Exception e)
        {
            await _failoverCommonLogic.Handle(request, e, cancellationToken);
            throw;
        }
    }
}
