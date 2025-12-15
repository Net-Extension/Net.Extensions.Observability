# Net.Extensions.Observability.Logging

Serilog-based implementation of logging for the Net.Extensions.Observability ecosystem. Provides production-ready structured logging with safe defaults.

## Features

- **Serilog Integration**: Full-featured Serilog implementation of `ILoggerAdapter`
- **Safe Defaults**: Pre-configured console logging with sensible settings
- **Flexible Configuration**: Easy-to-use options for customizing logging behavior
- **Multiple Sinks**: Support for console and file logging out of the box
- **Enrichers**: Automatic enrichment with machine name, environment, thread information
- **Rolling Files**: Configurable file rolling by time and size
- **Extension Methods**: Convenient fluent API for logger creation

## Installation

```bash
dotnet add package Net.Extensions.Observability.Logging
```

## Quick Start

### Basic Usage with Defaults

```csharp
using Net.Extensions.Observability.Logging;

// Create logger with safe defaults (console only)
var logger = LoggingExtensions.CreateLogger();

logger.Information("Application started");
logger.Debug("Debug information: {Detail}", someDetail);
logger.Error(exception, "An error occurred");
```

### Custom Configuration

```csharp
using Net.Extensions.Observability.Logging;

var logger = LoggingExtensions.CreateLogger(options =>
{
    options.MinimumLevel = "Debug";
    options.WriteToConsole = true;
    options.WriteToFile = true;
    options.FilePath = "logs/myapp-.log";
    options.RollingInterval = "Day";
    options.RetainedFileCount = 7;
});

logger.Information("Logger configured with custom settings");
```

### Using Existing Serilog Logger

```csharp
using Serilog;
using Net.Extensions.Observability.Logging;

var serilogLogger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Convert to ILoggerAdapter
var adapter = serilogLogger.ToAdapter();

adapter.Information("Using existing Serilog logger");
```

## Configuration Options

The `LoggingOptions` class provides the following configuration:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `MinimumLevel` | string | "Information" | Minimum log level (Debug, Information, Warning, Error, Fatal) |
| `WriteToConsole` | bool | true | Enable console logging |
| `WriteToFile` | bool | false | Enable file logging |
| `FilePath` | string | "logs/app-.log" | Path template for log files |
| `RollingInterval` | string | "Day" | Rolling interval (Infinite, Year, Month, Day, Hour, Minute) |
| `FileSizeLimitBytes` | long? | 100MB | Maximum file size before rolling |
| `RetainedFileCount` | int? | 31 | Number of log files to retain |
| `EnrichWithEnvironment` | bool | true | Add machine name, environment, user name |
| `EnrichWithThread` | bool | true | Add thread ID and name |
| `ConsoleOutputTemplate` | string? | null | Custom console output template |
| `FileOutputTemplate` | string? | null | Custom file output template |

## Advanced Usage

### Custom Output Templates

```csharp
var logger = LoggingExtensions.CreateLogger(options =>
{
    options.WriteToConsole = true;
    options.ConsoleOutputTemplate = 
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
});
```

### File Logging Only

```csharp
var logger = LoggingExtensions.CreateLogger(options =>
{
    options.WriteToConsole = false;
    options.WriteToFile = true;
    options.FilePath = "logs/production-.log";
    options.RollingInterval = "Hour";
    options.FileSizeLimitBytes = 50 * 1024 * 1024; // 50MB
    options.RetainedFileCount = 168; // 1 week of hourly logs
});
```

### Production Configuration

```csharp
var logger = LoggingExtensions.CreateLogger(options =>
{
    options.MinimumLevel = "Information";
    options.WriteToConsole = true;
    options.WriteToFile = true;
    options.FilePath = "/var/log/myapp/app-.log";
    options.RollingInterval = "Day";
    options.FileSizeLimitBytes = 100 * 1024 * 1024;
    options.RetainedFileCount = 30;
    options.EnrichWithEnvironment = true;
    options.EnrichWithThread = true;
});
```

## Default Console Output Format

By default, console logs use the following format:
```
[12:34:56 INF] Application started
[12:34:57 ERR] An error occurred
System.InvalidOperationException: Something went wrong
   at MyApp.Program.Main()
```

## Dependencies

- **Net.Extensions.Observability.Core** - Core abstractions
- **Serilog** (4.2.0) - Structured logging library
- **Serilog.Sinks.Console** (6.0.0) - Console output
- **Serilog.Sinks.File** (6.0.0) - File output
- **Serilog.Enrichers.Environment** (3.0.1) - Environment enrichment
- **Serilog.Enrichers.Thread** (4.0.0) - Thread enrichment

## Best Practices

1. **Use Structured Logging**: Pass parameters separately instead of string interpolation
   ```csharp
   // Good
   logger.Information("User {UserId} logged in", userId);
   
   // Avoid
   logger.Information($"User {userId} logged in");
   ```

2. **Configure Log Levels Appropriately**:
   - Development: `Debug`
   - Staging: `Information`
   - Production: `Information` or `Warning`

3. **Implement Log Retention**: Configure `RetainedFileCount` to prevent disk space issues

4. **Use File Logging in Production**: Enable file logging for production environments

5. **Monitor Log File Sizes**: Configure `FileSizeLimitBytes` appropriately for your environment

## Related Packages

- `Net.Extensions.Observability.Core` - Core abstractions and interfaces
- `Net.Extensions.Observability.Metrics` - Metrics recording
- `Net.Extensions.Observability.Tracing` - Distributed tracing
- `Net.Extensions.Observability.Health` - Health checks
