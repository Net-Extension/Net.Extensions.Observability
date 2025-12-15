using Net.Extensions.Observability.Core.Logging;
using Serilog;
using Serilog.Events;

namespace Net.Extensions.Observability.Logging;

/// <summary>
/// Serilog-based implementation of ILoggerAdapter.
/// </summary>
public sealed class SerilogAdapter : ILoggerAdapter
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of SerilogAdapter.
    /// </summary>
    public SerilogAdapter(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Log(Core.Logging.LogLevel level, string message, params object[] args)
    {
        _logger.Write(ConvertLogLevel(level), message, args);
    }

    /// <inheritdoc />
    public void Log(Core.Logging.LogLevel level, Exception exception, string message, params object[] args)
    {
        _logger.Write(ConvertLogLevel(level), exception, message, args);
    }

    /// <inheritdoc />
    public void Debug(string message, params object[] args)
    {
        _logger.Debug(message, args);
    }

    /// <inheritdoc />
    public void Information(string message, params object[] args)
    {
        _logger.Information(message, args);
    }

    /// <inheritdoc />
    public void Warning(string message, params object[] args)
    {
        _logger.Warning(message, args);
    }

    /// <inheritdoc />
    public void Error(string message, params object[] args)
    {
        _logger.Error(message, args);
    }

    /// <inheritdoc />
    public void Error(Exception exception, string message, params object[] args)
    {
        _logger.Error(exception, message, args);
    }

    /// <inheritdoc />
    public void Fatal(string message, params object[] args)
    {
        _logger.Fatal(message, args);
    }

    /// <inheritdoc />
    public void Fatal(Exception exception, string message, params object[] args)
    {
        _logger.Fatal(exception, message, args);
    }

    private static LogEventLevel ConvertLogLevel(Core.Logging.LogLevel level) => level switch
    {
        Core.Logging.LogLevel.Debug => LogEventLevel.Debug,
        Core.Logging.LogLevel.Information => LogEventLevel.Information,
        Core.Logging.LogLevel.Warning => LogEventLevel.Warning,
        Core.Logging.LogLevel.Error => LogEventLevel.Error,
        Core.Logging.LogLevel.Fatal => LogEventLevel.Fatal,
        _ => LogEventLevel.Information
    };
}
