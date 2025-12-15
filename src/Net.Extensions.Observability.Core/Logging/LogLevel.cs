namespace Net.Extensions.Observability.Core.Logging;

/// <summary>
/// Defines the log levels for logging messages.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Detailed information for debugging purposes.
    /// </summary>
    Debug = 0,

    /// <summary>
    /// Informational messages that highlight the progress of the application.
    /// </summary>
    Information = 1,

    /// <summary>
    /// Warning messages that indicate potential issues.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Error messages that indicate failures.
    /// </summary>
    Error = 3,

    /// <summary>
    /// Critical error messages that indicate severe failures.
    /// </summary>
    Fatal = 4
}
