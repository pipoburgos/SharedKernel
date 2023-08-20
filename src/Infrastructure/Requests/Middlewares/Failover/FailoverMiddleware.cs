using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Domain.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Failover;

/// <summary>  </summary>
public class FailoverMiddleware<TRequest> : IMiddleware<TRequest> where TRequest : IRequest
{
    private readonly FailoverCommonLogic _failoverCommonLogic;

    /// <summary>  </summary>
    public FailoverMiddleware(FailoverCommonLogic failoverCommonLogic)
    {
        _failoverCommonLogic = failoverCommonLogic;
    }

    /// <summary>  </summary>
    public async Task Handle(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next)
    {
        try
        {
            await next(request, cancellationToken);
        }
        catch (Exception e)
        {
            await _failoverCommonLogic.Handle(request, e, cancellationToken);

            throw;
        }
    }
}
