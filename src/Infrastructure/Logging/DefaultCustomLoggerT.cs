using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Logging;

/// <summary> . </summary>
public class DefaultCustomLogger<T> : ICustomLogger<T>
{
    private readonly ILogger<T> _logger;

    /// <summary> . </summary>
    public DefaultCustomLogger(ILogger<T> logger)
    {
        _logger = logger;
    }

    /// <summary> . </summary>
    public void Verbose(string message, params object[] args)
    {
        _logger.LogTrace(message, args);
    }

    /// <summary> . </summary>
    public void Debug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }

    /// <summary> . </summary>
    public void Info(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    /// <summary> . </summary>
    public void Warn(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    /// <summary> . </summary>
    public void Warn(Exception exception, string message, params object[] args)
    {
        _logger.LogWarning(exception, message, args);
    }

    /// <summary> . </summary>
    public void Error(Exception exception, string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    /// <summary> . </summary>
    public void Fatal(Exception exception, string message, params object[] args)
    {
        _logger.LogCritical(exception, message, args);
    }
}