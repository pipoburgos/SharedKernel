using SharedKernel.Application.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Failover;

/// <summary>  </summary>
public class FailoverMiddleware : IMiddleware
{
    private readonly FailoverCommonLogic _failoverCommonLogic;

    /// <summary>  </summary>
    public FailoverMiddleware(FailoverCommonLogic failoverCommonLogic)
    {
        _failoverCommonLogic = failoverCommonLogic;
    }

    /// <summary>  </summary>
    public async Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next) where TRequest : IRequest
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

    /// <summary>  </summary>
    public async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next) where TRequest : IRequest<TResponse>
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

    /// <summary>  </summary>
    public async Task<Result<TResponse>> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<Result<TResponse>>> next)
        where TRequest : IRequest<Result<TResponse>>
    {
        try
        {
            var result = await next(request, cancellationToken);

            if (result.IsFailure)
                await _failoverCommonLogic.Handle(request,
                    new Exception(string.Join(", ", result.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}"))),
                    cancellationToken);

            return result;
        }
        catch (Exception e)
        {
            await _failoverCommonLogic.Handle(request, e, cancellationToken);
            throw;
        }
    }
}
