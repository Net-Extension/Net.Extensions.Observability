# Net.Extensions.Observability.Core

Core abstractions and interfaces for the Net.Extensions.Observability ecosystem. This package contains only BCL-based interfaces and helpers with **no third-party dependencies**.

## Features

- **Logging Abstractions**: Generic logger interface (`ILoggerAdapter`) for structured logging
- **Metrics Abstractions**: Generic metrics recorder interface (`IMetricsRecorder`) for capturing application metrics
- **Tracing Abstractions**: Generic tracer interface (`ITracer`) for distributed tracing
- **Health Check Abstractions**: Generic health check interface (`IHealthCheck`) for application health monitoring
- **W3C Trace Context Support**: Built-in support for W3C Trace Context correlation headers
- **Noop Implementations**: Safe default implementations that do nothing (useful for testing and fallback scenarios)

## Usage

### Logging

```csharp
using Net.Extensions.Observability.Core.Logging;

public class MyService
{
    private readonly ILoggerAdapter _logger;

    public MyService(ILoggerAdapter logger)
    {
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.Information("Starting work");
        try
        {
            // Do work
            _logger.Debug("Work details: {Detail}", detail);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Work failed");
            throw;
        }
    }
}
```

### Metrics

```csharp
using Net.Extensions.Observability.Core.Metrics;

public class MyService
{
    private readonly IMetricsRecorder _metrics;

    public MyService(IMetricsRecorder metrics)
    {
        _metrics = metrics;
    }

    public void ProcessRequest()
    {
        _metrics.IncrementCounter("requests.processed");
        
        using (_metrics.StartTimer("request.duration"))
        {
            // Process request
        }
    }
}
```

### Tracing

```csharp
using Net.Extensions.Observability.Core.Tracing;

public class MyService
{
    private readonly ITracer _tracer;

    public MyService(ITracer tracer)
    {
        _tracer = tracer;
    }

    public void ProcessOrder(string orderId)
    {
        using var span = _tracer.StartSpan("process-order", SpanKind.Internal);
        span.SetAttribute("order.id", orderId);
        
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
}
```

### Health Checks

```csharp
using Net.Extensions.Observability.Core.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    public string Name => "Database";

    public async Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Check database connection
            await CheckDatabaseAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is not accessible", ex);
        }
    }
}
```

### W3C Trace Context

```csharp
using Net.Extensions.Observability.Core.Correlation;

// Parse incoming traceparent header
var traceparent = "00-0af7651916cd43dd8448eb211c80319c-b7ad6b7169203331-01";
var parsed = W3CTraceContext.ParseTraceParent(traceparent);
if (parsed.HasValue)
{
    var (version, traceId, spanId, flags) = parsed.Value;
    // Use correlation information
}

// Create outgoing traceparent header
var traceId = W3CTraceContext.GenerateTraceId();
var spanId = W3CTraceContext.GenerateSpanId();
var header = W3CTraceContext.CreateTraceParent(traceId, spanId, flags: 1);
```

## Noop Implementations

All interfaces have corresponding Noop implementations that are safe to use as defaults:

- `NoopLogger.Instance` - Does nothing for all log calls
- `NoopMetricsRecorder.Instance` - Does nothing for all metric recordings
- `NoopTracer.Instance` - Returns noop spans for all tracing operations
- `NoopHealthCheck.Instance` - Always returns healthy status
- `NoopCorrelationContext.Instance` - Does nothing for correlation operations

These are useful for testing, optional dependencies, or when observability features are disabled.

## Package Information

- **No third-party dependencies** - Uses only .NET BCL types
- **Target Framework**: .NET 9.0
- **Nullable Reference Types**: Enabled

## Related Packages

- `Net.Extensions.Observability.Logging` - Serilog-based logging implementation
- `Net.Extensions.Observability.Metrics` - OpenTelemetry-based metrics implementation
- `Net.Extensions.Observability.Tracing` - OpenTelemetry-based tracing implementation
- `Net.Extensions.Observability.Health` - Health check implementation
