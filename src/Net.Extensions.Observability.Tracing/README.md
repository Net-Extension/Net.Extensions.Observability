# Net.Extensions.Observability.Tracing

OpenTelemetry-based implementation of distributed tracing for the Net.Extensions.Observability ecosystem. Provides production-ready tracing with W3C Trace Context support and safe defaults.

## Features

- **OpenTelemetry Integration**: Full-featured OpenTelemetry implementation of `ITracer`
- **W3C Trace Context**: Built-in support for W3C Trace Context propagation
- **Safe Defaults**: Pre-configured tracing with sensible settings
- **Multiple Span Types**: Support for internal, server, client, producer, and consumer spans
- **Flexible Configuration**: Easy-to-use options for customizing tracing behavior
- **Console Exporter**: Built-in console exporter for development and debugging
- **Sampling Support**: Configurable sampling ratios for production environments
- **Resource Attributes**: Configurable service name, version, and custom attributes
- **Extension Methods**: Convenient fluent API for tracer creation

## Installation

```bash
dotnet add package Net.Extensions.Observability.Tracing
```

## Quick Start

### Basic Usage with Defaults

```csharp
using Net.Extensions.Observability.Tracing;

// Create tracer
var tracer = TracingExtensions.CreateTracer("MyApp.Tracing");

// Start a span
using (var span = tracer.StartSpan("process-order", SpanKind.Internal))
{
    span.SetAttribute("order.id", "12345");
    span.SetAttribute("order.amount", 99.99);
    
    try
    {
        // Process order
        span.AddEvent("Processing started");
        
        // ... processing logic ...
        
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

### Custom Configuration

```csharp
using Net.Extensions.Observability.Tracing;

var tracer = TracingExtensions.CreateTracer(
    "MyApp.Tracing",
    options =>
    {
        options.ServiceName = "OrderService";
        options.ServiceVersion = "2.0.0";
        options.ExportToConsole = true;
        options.SamplingRatio = 0.1; // 10% sampling for production
        options.EnableW3CTraceContext = true;
        options.ResourceAttributes["environment"] = "production";
        options.ResourceAttributes["region"] = "us-east-1";
    });
```

### Nested Spans

```csharp
using (var parentSpan = tracer.StartSpan("parent-operation"))
{
    parentSpan.SetAttribute("user.id", "user123");
    
    // Child span automatically inherits trace context
    using (var childSpan = tracer.StartChildSpan("database-query", SpanKind.Client))
    {
        childSpan.SetAttribute("db.system", "postgresql");
        childSpan.SetAttribute("db.statement", "SELECT * FROM orders");
        
        // Execute query
    }
    
    parentSpan.SetStatus(SpanStatus.Ok);
}
```

## Configuration Options

The `TracingOptions` class provides the following configuration:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ServiceName` | string | "MyService" | Service name for resource attributes |
| `ServiceVersion` | string | "1.0.0" | Service version for resource attributes |
| `ExportToConsole` | bool | false | Enable console exporter for traces |
| `SamplingRatio` | double | 1.0 | Sampling ratio (0.0 to 1.0, 1.0 = 100%) |
| `EnableW3CTraceContext` | bool | true | Enable W3C trace context propagation |
| `ResourceAttributes` | Dictionary | Empty | Additional resource attributes |

## W3C Trace Context

This library automatically supports W3C Trace Context propagation through the underlying OpenTelemetry Activity API. The trace context is propagated via `traceparent` and `tracestate` headers.

### Accessing Trace Context

```csharp
using (var span = tracer.StartSpan("operation"))
{
    var context = span.Context;
    Console.WriteLine($"Trace ID: {context.TraceId}");
    Console.WriteLine($"Span ID: {context.SpanId}");
    Console.WriteLine($"Trace Flags: {context.TraceFlags}");
}
```

### Manual W3C Trace Context Handling

For manual trace context handling, use the Core library's W3CTraceContext helper:

```csharp
using Net.Extensions.Observability.Core.Correlation;

// Parse incoming traceparent
var parsed = W3CTraceContext.ParseTraceParent(traceparentHeader);

// Create outgoing traceparent
var traceId = W3CTraceContext.GenerateTraceId();
var spanId = W3CTraceContext.GenerateSpanId();
var header = W3CTraceContext.CreateTraceParent(traceId, spanId, flags: 1);
```

## Span Operations

### Setting Attributes

```csharp
span.SetAttribute("http.method", "GET");
span.SetAttribute("http.url", "/api/orders");
span.SetAttribute("http.status_code", 200);
```

### Adding Events

```csharp
span.AddEvent("cache-miss");

span.AddEvent("retry-attempt", 
    new KeyValuePair<string, object?>("attempt", 2),
    new KeyValuePair<string, object?>("delay_ms", 100));
```

### Setting Status

```csharp
// Success
span.SetStatus(SpanStatus.Ok);

// Error
span.SetStatus(SpanStatus.Error, "Validation failed");

// Unset (default)
span.SetStatus(SpanStatus.Unset);
```

### Recording Exceptions

```csharp
try
{
    // Operation
}
catch (Exception ex)
{
    span.RecordException(ex);
    span.SetStatus(SpanStatus.Error);
    throw;
}
```

## Span Kinds

Choose the appropriate span kind based on the operation:

```csharp
// Internal operation (default)
var span1 = tracer.StartSpan("calculate", SpanKind.Internal);

// Server-side operation (handling incoming request)
var span2 = tracer.StartSpan("handle-request", SpanKind.Server);

// Client-side operation (making outgoing request)
var span3 = tracer.StartSpan("http-call", SpanKind.Client);

// Producer operation (sending message)
var span4 = tracer.StartSpan("publish-message", SpanKind.Producer);

// Consumer operation (receiving message)
var span5 = tracer.StartSpan("consume-message", SpanKind.Consumer);
```

## Advanced Usage

### Creating TracerProvider Separately

```csharp
using System.Diagnostics;
using OpenTelemetry.Trace;

// Create tracer provider with custom configuration
var tracerProvider = TracingExtensions.CreateTracerProvider(
    "MyApp.Tracing",
    options =>
    {
        options.ServiceName = "MyService";
        options.SamplingRatio = 0.1;
        options.ExportToConsole = true;
    });

// Create activity source and tracer
var activitySource = TracingExtensions.CreateActivitySource("MyApp.Tracing");
var tracer = activitySource.ToTracer();
```

### Using System.Diagnostics Directly

```csharp
using System.Diagnostics;

var activitySource = new ActivitySource("MyApp.Tracing");
var tracer = activitySource.ToTracer();

// Use tracer
using var span = tracer.StartSpan("operation");
```

## Best Practices

1. **Use Meaningful Span Names**: Names should describe the operation
   ```csharp
   using var span = tracer.StartSpan("database.query.users");
   ```

2. **Add Relevant Attributes**: Include contextual information
   ```csharp
   span.SetAttribute("user.id", userId);
   span.SetAttribute("endpoint", "/api/users");
   ```

3. **Always Dispose Spans**: Use `using` statements to ensure proper cleanup
   ```csharp
   using (var span = tracer.StartSpan("operation"))
   {
       // Work
   } // Span automatically ended and exported
   ```

4. **Handle Exceptions Properly**: Record exceptions and set error status
   ```csharp
   try
   {
       // Operation
   }
   catch (Exception ex)
   {
       span.RecordException(ex);
       span.SetStatus(SpanStatus.Error, ex.Message);
       throw;
   }
   ```

5. **Configure Sampling for Production**: Reduce overhead in high-traffic scenarios
   ```csharp
   options.SamplingRatio = 0.1; // 10% sampling
   ```

6. **Use Appropriate Span Kinds**: Choose the right kind for better trace visualization

## Known Issues

OpenTelemetry.Api has a known moderate severity vulnerability (GHSA-8785-wc3w-h8q6). This is being tracked by the OpenTelemetry team. The vulnerability relates to resource consumption and should be considered when deploying to production.

## Dependencies

- **Net.Extensions.Observability.Core** - Core abstractions
- **OpenTelemetry** (1.10.0) - OpenTelemetry SDK
- **OpenTelemetry.Api** (1.11.1) - OpenTelemetry API
- **OpenTelemetry.Exporter.Console** (1.10.0) - Console exporter

## Related Packages

- `Net.Extensions.Observability.Core` - Core abstractions and interfaces (includes W3C utilities)
- `Net.Extensions.Observability.Logging` - Structured logging
- `Net.Extensions.Observability.Metrics` - Metrics recording
- `Net.Extensions.Observability.Health` - Health checks
