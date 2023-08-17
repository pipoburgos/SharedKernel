using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Requests;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Timer;

/// <summary>  </summary>
public class TimerMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
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
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next)

    {
        _timer.Start();

        var response = await next(request, cancellationToken);

        _timer.Stop();

        _timeHandler.Handle(request, _timer);

        return response;
    }
}