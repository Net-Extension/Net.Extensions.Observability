using System.Diagnostics;
using Net.Extensions.Observability.Core.Tracing;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Net.Extensions.Observability.Tracing;

/// <summary>
/// Extension methods for configuring tracing.
/// </summary>
public static class TracingExtensions
{
    /// <summary>
    /// Creates a TracerProvider with safe defaults and W3C trace context support.
    /// </summary>
    public static TracerProvider CreateTracerProvider(string sourceName, Action<TracingOptions>? configure = null)
    {
        var options = new TracingOptions();
        configure?.Invoke(options);

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(options.ServiceName, serviceVersion: options.ServiceVersion);

        // Add custom resource attributes if present
        if (options.ResourceAttributes.Count > 0)
        {
            resourceBuilder = resourceBuilder.AddAttributes(
                options.ResourceAttributes.Select(kv => new KeyValuePair<string, object>(kv.Key, kv.Value)));
        }

        var builder = Sdk.CreateTracerProviderBuilder()
            .AddSource(sourceName)
            .SetResourceBuilder(resourceBuilder);

        // Configure sampling
        if (options.SamplingRatio < 1.0)
        {
            builder.SetSampler(new TraceIdRatioBasedSampler(options.SamplingRatio));
        }

        if (options.ExportToConsole)
        {
            builder.AddConsoleExporter();
        }

        return builder.Build()!;
    }

    /// <summary>
    /// Creates an ActivitySource with the specified name.
    /// </summary>
    public static ActivitySource CreateActivitySource(string sourceName, string? version = null)
    {
        return new ActivitySource(sourceName, version);
    }

    /// <summary>
    /// Creates an ITracer with OpenTelemetry and safe defaults with W3C trace context support.
    /// </summary>
    public static ITracer CreateTracer(
        string sourceName,
        Action<TracingOptions>? configure = null,
        TracerProvider? tracerProvider = null)
    {
        // Create tracer provider if not provided
        if (tracerProvider == null && configure != null)
        {
            tracerProvider = CreateTracerProvider(sourceName, configure);
        }

        var activitySource = CreateActivitySource(sourceName);
        return new OpenTelemetryTracer(activitySource);
    }

    /// <summary>
    /// Wraps an existing ActivitySource as an ITracer.
    /// </summary>
    public static ITracer ToTracer(this ActivitySource activitySource)
    {
        return new OpenTelemetryTracer(activitySource);
    }
}
