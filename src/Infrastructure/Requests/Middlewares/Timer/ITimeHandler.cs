using System.Diagnostics;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Timer;

/// <summary>  </summary>
public interface ITimeHandler
{
    /// <summary>  </summary>
    public void Handle<TRequest>(TRequest request, Stopwatch timer);
}