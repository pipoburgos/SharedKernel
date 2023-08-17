using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Domain.Requests;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public class TimerMiddleware<TRequest> : IMiddleware<TRequest> where TRequest : IRequest
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
    public async Task Handle(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next)
    {
        _timer.Start();

        await next(request, cancellationToken);

        _timer.Stop();

        _timeHandler.Handle(request, _timer);
    }
}
