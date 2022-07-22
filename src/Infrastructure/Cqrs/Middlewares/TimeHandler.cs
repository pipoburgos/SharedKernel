using SharedKernel.Application.Logging;
using System.Diagnostics;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares;

/// <summary>  </summary>
public class TimeHandler : ITimeHandler
{
    private readonly ICustomLogger<TimeHandler> _logger;
    private readonly int _milliseconds;

    /// <summary> Constructor. </summary>
    public TimeHandler(ICustomLogger<TimeHandler> logger, int milliseconds)
    {
        _logger = logger;
        _milliseconds = milliseconds;
    }

    /// <summary>  </summary>
    public void Handle<TRequest>(TRequest request, Stopwatch timer)
    {
        var name = typeof(TRequest).Name;

        if (timer.ElapsedMilliseconds > _milliseconds)
            _logger.Warn($"TimerBehaviour: {name} ({timer.ElapsedMilliseconds} milliseconds) {request}");
    }
}