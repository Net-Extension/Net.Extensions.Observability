# Net.Extensions.Observability

A comprehensive observability ecosystem for .NET applications, providing production-ready implementations of logging, metrics, tracing, and health checks with safe defaults and W3C Trace Context support.

## Overview

The Net.Extensions.Observability ecosystem consists of five packages that work together to provide complete observability for your .NET applications:

| Package | Description | Dependencies |
|---------|-------------|--------------|
| **Net.Extensions.Observability.Core** | Core abstractions and interfaces (BCL only) | None (BCL only) |
| **Net.Extensions.Observability.Logging** | Serilog-based structured logging | Serilog |
| **Net.Extensions.Observability.Metrics** | OpenTelemetry-based metrics recording | OpenTelemetry |
| **Net.Extensions.Observability.Tracing** | OpenTelemetry-based distributed tracing with W3C support | OpenTelemetry |
| **Net.Extensions.Observability.Health** | Health check implementation | Microsoft.Extensions.Diagnostics.HealthChecks |

## Key Features

- ✅ **BCL-based Core**: Core package with zero third-party dependencies
- ✅ **Safe Defaults**: All implementations come with production-ready defaults
- ✅ **W3C Trace Context**: Built-in support for W3C Trace Context propagation
- ✅ **Noop Implementations**: Safe fallback implementations for all interfaces
- ✅ **Extension Methods**: Fluent, easy-to-use APIs
- ✅ **Options Classes**: Configurable behavior for all components
- ✅ **No Host Wiring**: Standalone libraries, no DI container required
- ✅ **Comprehensive Documentation**: Each package includes detailed README

## Quick Start

### Installation

```bash
# Install all packages
dotnet add package Net.Extensions.Observability.Core
dotnet add package Net.Extensions.Observability.Logging
dotnet add package Net.Extensions.Observability.Metrics
dotnet add package Net.Extensions.Observability.Tracing
dotnet add package Net.Extensions.Observability.Health

# Or install only what you need
dotnet add package Net.Extensions.Observability.Logging
```

### Logging

```csharp
using Net.Extensions.Observability.Logging;

var logger = LoggingExtensions.CreateLogger(options =>
{
    options.MinimumLevel = "Information";
    options.WriteToConsole = true;
    options.WriteToFile = true;
    options.FilePath = "logs/app-.log";
});

logger.Information("Application started");
logger.Error(exception, "An error occurred");
```

### Metrics

```csharp
using Net.Extensions.Observability.Metrics;

var recorder = MetricsExtensions.CreateMetricsRecorder("MyApp.Metrics", options =>
{
    options.ServiceName = "MyService";
    options.ExportToConsole = true;
});

recorder.IncrementCounter("requests.total");
recorder.RecordHistogram("request.duration", 123.45);

using (recorder.StartTimer("operation.duration"))
{
    // Operation is timed automatically
}
```

### Tracing

```csharp
using Net.Extensions.Observability.Tracing;

var tracer = TracingExtensions.CreateTracer("MyApp.Tracing", options =>
{
    options.ServiceName = "MyService";
    options.EnableW3CTraceContext = true;
    options.ExportToConsole = true;
});

using (var span = tracer.StartSpan("process-order", SpanKind.Internal))
{
    span.SetAttribute("order.id", "12345");
    
    try
    {
        // Process order
        span.SetStatus(SpanStatus.Ok);
    }
    catch (Exception ex)
    {
        span.RecordException(ex);
        span.SetStatus(SpanStatus.Error, ex.Message);
        throw;
    }
}
```

### Health Checks

```csharp
using Net.Extensions.Observability.Health;

var healthChecks = new[]
{
    BasicHealthChecks.FileExists("/app/config.json", "Config"),
    BasicHealthChecks.DirectoryExists("/app/data", "Data")
};

var result = await healthChecks.CheckAllAsync();
Console.WriteLine($"Overall Status: {result.Status}");
```

## Architecture

The ecosystem follows a layered architecture:

```
┌─────────────────────────────────────────────────────────┐
│                    Your Application                     │
└─────────────────────────────────────────────────────────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
        ▼                  ▼                  ▼
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│   Logging    │  │   Metrics    │  │   Tracing    │  ...
│  (Serilog)   │  │(OpenTelemetry)│  │(OpenTelemetry)│
└──────────────┘  └──────────────┘  └──────────────┘
        │                  │                  │
        └──────────────────┼──────────────────┘
                           ▼
        ┌──────────────────────────────────────┐
        │  Net.Extensions.Observability.Core   │
        │      (BCL-based Interfaces)          │
        └──────────────────────────────────────┘
```

## Package Details

### Net.Extensions.Observability.Core

The foundation package containing:
- `ILoggerAdapter` - Generic logging interface
- `IMetricsRecorder` - Generic metrics interface
- `ITracer`, `ISpan` - Generic tracing interfaces
- `IHealthCheck` - Generic health check interface
- `ICorrelationContext` - W3C Trace Context support
- W3C Trace Context utilities
- Noop implementations for all interfaces

**No third-party dependencies** - Uses only .NET BCL types.

[View Core README](src/Net.Extensions.Observability.Core/README.md)

### Net.Extensions.Observability.Logging

Serilog-based implementation featuring:
- `SerilogAdapter` - Bridges Serilog to `ILoggerAdapter`
- Console and file sinks with safe defaults
- Automatic enrichment (machine name, environment, thread info)
- Configurable log levels, output templates, rolling files
- Extension methods for easy logger creation

**Dependencies**: Serilog, Serilog.Sinks.Console, Serilog.Sinks.File, Serilog.Enrichers.*

[View Logging README](src/Net.Extensions.Observability.Logging/README.md)

### Net.Extensions.Observability.Metrics

OpenTelemetry-based implementation featuring:
- `OpenTelemetryMetricsRecorder` - Bridges OpenTelemetry to `IMetricsRecorder`
- Support for counters, gauges, histograms, timers
- Console exporter for development
- Configurable resource attributes and service information
- Extension methods for easy metrics creation

**Dependencies**: OpenTelemetry, OpenTelemetry.Api, OpenTelemetry.Exporter.Console

[View Metrics README](src/Net.Extensions.Observability.Metrics/README.md)

### Net.Extensions.Observability.Tracing

OpenTelemetry-based implementation featuring:
- `OpenTelemetryTracer` - Bridges OpenTelemetry to `ITracer`
- Full W3C Trace Context propagation support
- Support for multiple span kinds (Internal, Server, Client, Producer, Consumer)
- Configurable sampling for production
- Console exporter for development
- Extension methods for easy tracer creation

**Dependencies**: OpenTelemetry, OpenTelemetry.Api, OpenTelemetry.Exporter.Console

[View Tracing README](src/Net.Extensions.Observability.Tracing/README.md)

### Net.Extensions.Observability.Health

Health check implementation featuring:
- `IHealthCheck` implementation
- Compatibility with Microsoft.Extensions.Diagnostics.HealthChecks
- Basic health checks (file, directory, custom)
- Aggregated health checks
- Configurable timeouts and detail levels
- Extension methods for health check operations

**Dependencies**: Microsoft.Extensions.Diagnostics.HealthChecks

[View Health README](src/Net.Extensions.Observability.Health/README.md)

## Design Principles

1. **No Host Wiring**: All packages work standalone without requiring a DI container or host builder
2. **Safe Defaults**: Every component has sensible default configurations
3. **Minimal Dependencies**: Core package has zero dependencies; implementation packages use only essential libraries
4. **W3C Standards**: Full support for W3C Trace Context propagation
5. **Noop Implementations**: Safe fallbacks when observability is disabled
6. **Easy to Use**: Extension methods and options classes for simple configuration
7. **Production Ready**: Designed for production use with performance and reliability in mind

## Known Issues

- OpenTelemetry.Api (versions 1.10.0 - 1.11.1) has a known moderate severity vulnerability (GHSA-8785-wc3w-h8q6) related to resource consumption. This is being tracked by the OpenTelemetry team.

## Target Framework

All packages target **.NET 9.0** with nullable reference types enabled.

## License

[Specify your license here]

## Contributing

[Specify contribution guidelines here]