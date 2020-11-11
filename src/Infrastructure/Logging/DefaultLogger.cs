using System;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Infrastructure.Logging
{
    public class DefaultCustomLogger<T> : Application.Logging.ICustomLogger<T>
    {
        private readonly ILogger<T> _logger;

        public DefaultCustomLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Verbose(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void Warn(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }
    }

    public class DefaultCustomLogger : Application.Logging.ICustomLogger
    {
        private readonly ILogger _logger;

        public DefaultCustomLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Verbose(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void Warn(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }
    }
}
