using SharedKernel.Application.Logging;
#if NET
using System.Diagnostics.CodeAnalysis;
#endif


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
    public void Verbose(
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogTrace(message, args);
    }

    /// <summary> . </summary>
    public void Debug(
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }

    /// <summary> . </summary>
    public void Info(
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    /// <summary> . </summary>
    public void Warn(
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    /// <summary> . </summary>
    public void Warn(Exception exception,
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogWarning(exception, message, args);
    }

    /// <summary> . </summary>
    public void Error(Exception exception,
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    /// <summary> . </summary>
    public void Fatal(Exception exception,
#if NET
        [StringSyntax("MessageTemplate")]
#endif
        string message, params object[] args)
    {
        _logger.LogCritical(exception, message, args);
    }
}