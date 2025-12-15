namespace Net.Extensions.Observability.Core.Logging;

/// <summary>
/// Defines a generic logger interface for structured logging.
/// </summary>
public interface ILoggerAdapter
{
    /// <summary>
    /// Logs a message at the specified log level.
    /// </summary>
    void Log(LogLevel level, string message, params object[] args);

    /// <summary>
    /// Logs a message with an exception at the specified log level.
    /// </summary>
    void Log(LogLevel level, Exception exception, string message, params object[] args);

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    void Debug(string message, params object[] args);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    void Information(string message, params object[] args);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    void Warning(string message, params object[] args);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    void Error(string message, params object[] args);

    /// <summary>
    /// Logs an error message with an exception.
    /// </summary>
    void Error(Exception exception, string message, params object[] args);

    /// <summary>
    /// Logs a fatal error message.
    /// </summary>
    void Fatal(string message, params object[] args);

    /// <summary>
    /// Logs a fatal error message with an exception.
    /// </summary>
    void Fatal(Exception exception, string message, params object[] args);
}
