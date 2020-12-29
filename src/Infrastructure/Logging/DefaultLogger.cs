using System;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Infrastructure.Logging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultCustomLogger<T> : Application.Logging.ICustomLogger<T>
    {
        private readonly ILogger<T> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public DefaultCustomLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Verbose(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultCustomLogger : Application.Logging.ICustomLogger
    {
        private readonly ILogger<DefaultCustomLogger> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public DefaultCustomLogger(ILogger<DefaultCustomLogger> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Verbose(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }
    }
}
