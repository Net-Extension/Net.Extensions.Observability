# Net.Extensions.Observability.Metrics

OpenTelemetry-based implementation of metrics recording for the Net.Extensions.Observability ecosystem. Provides production-ready metrics collection with safe defaults.

## Features

- **OpenTelemetry Integration**: Full-featured OpenTelemetry implementation of `IMetricsRecorder`
- **Safe Defaults**: Pre-configured metrics with sensible settings
- **Multiple Metric Types**: Support for counters, gauges, histograms, and timers
- **Flexible Configuration**: Easy-to-use options for customizing metrics behavior
- **Console Exporter**: Built-in console exporter for development and debugging
- **Resource Attributes**: Configurable service name, version, and custom attributes
- **Extension Methods**: Convenient fluent API for metrics creation

## Installation

```bash
dotnet add package Net.Extensions.Observability.Metrics
```

## Quick Start

### Basic Usage with Defaults

```csharp
using Net.Extensions.Observability.Metrics;

// Create metrics recorder
var recorder = MetricsExtensions.CreateMetricsRecorder("MyApp.Metrics");

// Record metrics
recorder.IncrementCounter("requests.total");
recorder.RecordHistogram("request.duration", 123.45);
recorder.RecordGauge("cpu.usage", 45.2);

// Use timer
using (recorder.StartTimer("operation.duration"))
{
    // Perform operation - duration will be recorded automatically
}
```

### Custom Configuration

```csharp
using Net.Extensions.Observability.Metrics;

var recorder = MetricsExtensions.CreateMetricsRecorder(
    "MyApp.Metrics",
    options =>
    {
        options.ServiceName = "MyService";
        options.ServiceVersion = "2.0.0";
        options.ExportToConsole = true;
        options.ConsoleExportIntervalMilliseconds = 5000; // 5 seconds
        options.ResourceAttributes["environment"] = "production";
        options.ResourceAttributes["region"] = "us-east-1";
    });

recorder.IncrementCounter("requests.total");
```

### Using with Tags

```csharp
// Record metrics with tags
recorder.IncrementCounter("requests.total", 
    new KeyValuePair<string, object?>("method", "GET"),
    new KeyValuePair<string, object?>("status", 200));

recorder.RecordHistogram("request.duration", 
    156.78,
    new KeyValuePair<string, object?>("endpoint", "/api/users"),
    new KeyValuePair<string, object?>("method", "POST"));
```

## Configuration Options

The `MetricsOptions` class provides the following configuration:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ServiceName` | string | "MyService" | Service name for resource attributes |
| `ServiceVersion` | string | "1.0.0" | Service version for resource attributes |
| `ExportToConsole` | bool | false | Enable console exporter for metrics |
| `ConsoleExportIntervalMilliseconds` | int | 10000 | Console export interval (10 seconds) |
| `ResourceAttributes` | Dictionary | Empty | Additional resource attributes |

## Metric Types

### Counter
Monotonically increasing value (e.g., request count, error count):
```csharp
recorder.RecordCounter("requests.processed", 5);
recorder.IncrementCounter("errors.total");
```

### Gauge
Current value that can go up or down (e.g., CPU usage, memory):
```csharp
recorder.RecordGauge("cpu.usage.percent", 45.2);
recorder.RecordGauge("memory.used.bytes", 1024000);
```

### Histogram
Distribution of values (e.g., request duration, payload size):
```csharp
recorder.RecordHistogram("request.duration.ms", 123.45);
recorder.RecordHistogram("response.size.bytes", 4096);
```

### Timer
Automatically measured duration:
```csharp
using (recorder.StartTimer("database.query.duration"))
{
    // Execute database query
    // Duration is recorded automatically when disposed
}
```

## Advanced Usage

### Creating MeterProvider Separately

```csharp
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;

// Create meter provider with custom configuration
var meterProvider = MetricsExtensions.CreateMeterProvider(
    "MyApp.Metrics",
    options =>
    {
        options.ServiceName = "MyService";
        options.ExportToConsole = true;
    });

// Create meter and recorder
var meter = MetricsExtensions.CreateMeter("MyApp.Metrics", "1.0.0");
var recorder = meter.ToRecorder();
```

### Using System.Diagnostics.Metrics Directly

```csharp
using System.Diagnostics.Metrics;

var meter = new Meter("MyApp.Metrics");
var recorder = meter.ToRecorder();

// Use recorder
recorder.IncrementCounter("operations.total");
```

## Best Practices

1. **Use Meaningful Metric Names**: Follow naming conventions like `component.metric.unit`
   ```csharp
   recorder.RecordHistogram("http.request.duration.ms", duration);
   recorder.IncrementCounter("database.queries.total");
   ```

2. **Add Relevant Tags**: Use tags to add dimensions to metrics
   ```csharp
   recorder.IncrementCounter("requests.total",
       new KeyValuePair<string, object?>("method", "GET"),
       new KeyValuePair<string, object?>("endpoint", "/api/users"));
   ```

3. **Use Appropriate Metric Types**:
   - Counter for monotonically increasing values
   - Histogram for distributions
   - Timer for durations

4. **Avoid High Cardinality Tags**: Don't use tags with many unique values (e.g., user IDs)
   ```csharp
   // Good
   recorder.IncrementCounter("requests", new("region", "us-east"));
   
   // Bad - too many unique values
   // recorder.IncrementCounter("requests", new("user_id", userId));
   ```

5. **Configure Export in Production**: Set up appropriate exporters (Prometheus, OTLP) for production

## Known Issues

OpenTelemetry.Api has a known moderate severity vulnerability (GHSA-8785-wc3w-h8q6). This is being tracked by the OpenTelemetry team. The vulnerability relates to resource consumption and should be considered when deploying to production.

## Dependencies

- **Net.Extensions.Observability.Core** - Core abstractions
- **OpenTelemetry** (1.10.0) - OpenTelemetry SDK
- **OpenTelemetry.Api** (1.11.1) - OpenTelemetry API
- **OpenTelemetry.Exporter.Console** (1.10.0) - Console exporter

## Related Packages

- `Net.Extensions.Observability.Core` - Core abstractions and interfaces
- `Net.Extensions.Observability.Logging` - Structured logging
- `Net.Extensions.Observability.Tracing` - Distributed tracing
- `Net.Extensions.Observability.Health` - Health checks
