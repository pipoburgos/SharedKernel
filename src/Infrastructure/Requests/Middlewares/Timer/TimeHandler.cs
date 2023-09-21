using System.Diagnostics;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Timer;

/// <summary>  </summary>
public class TimeHandler : ITimeHandler
{
    private readonly ILogger<TimeHandler> _logger;
    private readonly int _milliseconds;

    /// <summary> Constructor. </summary>
    public TimeHandler(ILogger<TimeHandler> logger, int milliseconds)
    {
        _logger = logger;
        _milliseconds = milliseconds;
    }

    /// <summary>  </summary>
    public void Handle<TRequest>(TRequest request, Stopwatch timer)
    {
        var name = typeof(TRequest).Name;

        if (timer.ElapsedMilliseconds > _milliseconds)
            _logger.LogWarning($"TimerBehaviour: {name} ({timer.ElapsedMilliseconds} milliseconds) {request}");
    }
}
