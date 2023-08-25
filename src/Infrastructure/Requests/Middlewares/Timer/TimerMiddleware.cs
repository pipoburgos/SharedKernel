using SharedKernel.Application.Cqrs.Middlewares;
using System.Diagnostics;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Timer;

/// <summary>  </summary>
public class TimerMiddleware : IMiddleware
{
    private readonly ITimeHandler _timeHandler;
    private readonly Stopwatch _timer;

    /// <summary> Constructor. </summary>
    public TimerMiddleware(ITimeHandler timeHandler)
    {
        _timeHandler = timeHandler;
        _timer = new Stopwatch();
    }

    /// <summary>  </summary>
    public async Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next) where TRequest : IRequest
    {
        _timer.Start();
        await next(request, cancellationToken);
        _timer.Stop();
        _timeHandler.Handle(request, _timer);
    }

    /// <summary>  </summary>
    public async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next) where TRequest : IRequest<TResponse>
    {
        _timer.Start();
        var response = await next(request, cancellationToken);
        _timer.Stop();
        _timeHandler.Handle(request, _timer);
        return response;
    }

    /// <summary>  </summary>
    public async Task<ApplicationResult<TResponse>> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<ApplicationResult<TResponse>>> next)
        where TRequest : IRequest<ApplicationResult<TResponse>>
    {
        _timer.Start();
        var result = await next(request, cancellationToken);
        _timer.Stop();
        _timeHandler.Handle(request, _timer);
        return result;
    }
}
