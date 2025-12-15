namespace Net.Extensions.Observability.Core.Logging;

/// <summary>
/// A no-operation logger implementation that does nothing.
/// </summary>
public sealed class NoopLogger : ILoggerAdapter
{
    /// <summary>
    /// Gets the singleton instance of the NoopLogger.
    /// </summary>
    public static readonly NoopLogger Instance = new();

    private NoopLogger() { }

    /// <inheritdoc />
    public void Log(LogLevel level, string message, params object[] args) { }

    /// <inheritdoc />
    public void Log(LogLevel level, Exception exception, string message, params object[] args) { }

    /// <inheritdoc />
    public void Debug(string message, params object[] args) { }

    /// <inheritdoc />
    public void Information(string message, params object[] args) { }

    /// <inheritdoc />
    public void Warning(string message, params object[] args) { }

    /// <inheritdoc />
    public void Error(string message, params object[] args) { }

    /// <inheritdoc />
    public void Error(Exception exception, string message, params object[] args) { }

    /// <inheritdoc />
    public void Fatal(string message, params object[] args) { }

    /// <inheritdoc />
    public void Fatal(Exception exception, string message, params object[] args) { }
}
