using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Requests;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Failover;

/// <summary>  </summary>
public class FailoverMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly FailoverCommonLogic _failoverCommonLogic;

    /// <summary>  </summary>
    public FailoverMiddleware(FailoverCommonLogic failoverCommonLogic)
    {
        _failoverCommonLogic = failoverCommonLogic;
    }

    /// <summary>  </summary>
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
