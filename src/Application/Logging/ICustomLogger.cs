//namespace SharedKernel.Application.Logging;

///// <summary>
///// Provides logging interface and utility functions.
///// https://github.com/uhaciogullari/NLog.Interface/blob/master/NLog.Interface/ICustomLogger.cs
///// </summary>
//public interface ICustomLogger
//{
//    /// <overloads>
//    /// Writes the diagnostic message at the <c>Info</c> level using the specified format provider and format parameters.
//    /// </overloads>
//    /// <summary>
//    /// Writes the diagnostic message at the <c>Info</c> level.
//    /// </summary>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Verbose(string message, params object[] args);

//    /// <overloads>
//    /// Writes the diagnostic message at the <c>Info</c> level using the specified format provider and format parameters.
//    /// </overloads>
//    /// <summary>
//    /// Writes the diagnostic message at the <c>Info</c> level.
//    /// </summary>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Debug(string message, params object[] args);

//    /// <overloads>
//    /// Writes the diagnostic message at the <c>Info</c> level using the specified format provider and format parameters.
//    /// </overloads>
//    /// <summary>
//    /// Writes the diagnostic message at the <c>Info</c> level.
//    /// </summary>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Info(string message, params object[] args);

//    /// <overloads>
//    /// Writes the diagnostic message at the <c>Warn</c> level using the specified format provider and format parameters.
//    /// </overloads>
//    /// <summary>
//    /// Writes the diagnostic message at the <c>Warn</c> level.
//    /// </summary>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Warn(string message, params object[] args);

//    /// <overloads>
//    /// Writes the diagnostic message at the <c>Warn</c> level using the specified format provider and format parameters.
//    /// </overloads>
//    /// <summary>
//    /// Writes the diagnostic message at the <c>Warn</c> level.
//    /// </summary>
//    /// <param name="exception"></param>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Warn(Exception exception, string message, params object[] args);

//    /// <summary>
//    /// Writes the diagnostic message and exception at the <c>Error</c> level.
//    /// </summary>
//    /// <param name="exception"></param>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Error(Exception exception, string message, params object[] args);

//    /// <overloads>
//    /// Writes the diagnostic message at the <c>Fatal</c> level using the specified format provider and format parameters.
//    /// </overloads>
//    /// <summary>
//    /// Writes the diagnostic message at the <c>Fatal</c> level.
//    /// </summary>
//    /// <param name="exception"></param>
//    /// <param name="message">The value to be written.</param>
//    /// <param name="args"></param>
//    void Fatal(Exception exception, string message, params object[] args);
//}