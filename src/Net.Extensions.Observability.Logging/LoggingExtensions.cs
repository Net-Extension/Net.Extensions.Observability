using Net.Extensions.Observability.Core.Logging;
using Serilog;
using Serilog.Events;

namespace Net.Extensions.Observability.Logging;

/// <summary>
/// Extension methods for configuring logging.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Creates a Serilog logger with safe defaults.
    /// </summary>
    public static ILogger CreateDefaultLogger(Action<LoggingOptions>? configure = null)
    {
        var options = new LoggingOptions();
        configure?.Invoke(options);

        var configuration = new LoggerConfiguration()
            .MinimumLevel.Is(ParseLogLevel(options.MinimumLevel));

        if (options.EnrichWithEnvironment)
        {
            configuration.Enrich.WithMachineName()
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithEnvironmentUserName();
        }

        if (options.EnrichWithThread)
        {
            configuration.Enrich.WithThreadId()
                        .Enrich.WithThreadName();
        }

        if (options.WriteToConsole)
        {
            if (!string.IsNullOrWhiteSpace(options.ConsoleOutputTemplate))
            {
                configuration.WriteTo.Console(outputTemplate: options.ConsoleOutputTemplate);
            }
            else
            {
                configuration.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
            }
        }

        if (options.WriteToFile)
        {
            var rollingInterval = ParseRollingInterval(options.RollingInterval);
            
            if (!string.IsNullOrWhiteSpace(options.FileOutputTemplate))
            {
                configuration.WriteTo.File(
                    options.FilePath,
                    rollingInterval: rollingInterval,
                    fileSizeLimitBytes: options.FileSizeLimitBytes,
                    retainedFileCountLimit: options.RetainedFileCount,
                    outputTemplate: options.FileOutputTemplate);
            }
            else
            {
                configuration.WriteTo.File(
                    options.FilePath,
                    rollingInterval: rollingInterval,
                    fileSizeLimitBytes: options.FileSizeLimitBytes,
                    retainedFileCountLimit: options.RetainedFileCount);
            }
        }

        return configuration.CreateLogger();
    }

    /// <summary>
    /// Creates an ILoggerAdapter with Serilog and safe defaults.
    /// </summary>
    public static ILoggerAdapter CreateLogger(Action<LoggingOptions>? configure = null)
    {
        var logger = CreateDefaultLogger(configure);
        return new SerilogAdapter(logger);
    }

    /// <summary>
    /// Wraps an existing Serilog logger as an ILoggerAdapter.
    /// </summary>
    public static ILoggerAdapter ToAdapter(this ILogger logger)
    {
        return new SerilogAdapter(logger);
    }

    private static LogEventLevel ParseLogLevel(string level) => level.ToLowerInvariant() switch
    {
        "debug" or "verbose" => LogEventLevel.Debug,
        "information" or "info" => LogEventLevel.Information,
        "warning" or "warn" => LogEventLevel.Warning,
        "error" => LogEventLevel.Error,
        "fatal" or "critical" => LogEventLevel.Fatal,
        _ => LogEventLevel.Information
    };

    private static RollingInterval ParseRollingInterval(string interval) => interval.ToLowerInvariant() switch
    {
        "infinite" => RollingInterval.Infinite,
        "year" => RollingInterval.Year,
        "month" => RollingInterval.Month,
        "day" => RollingInterval.Day,
        "hour" => RollingInterval.Hour,
        "minute" => RollingInterval.Minute,
        _ => RollingInterval.Day
    };
}
