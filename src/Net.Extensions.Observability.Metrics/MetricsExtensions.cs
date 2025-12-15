using System.Diagnostics.Metrics;
using Net.Extensions.Observability.Core.Metrics;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Net.Extensions.Observability.Metrics;

/// <summary>
/// Extension methods for configuring metrics.
/// </summary>
public static class MetricsExtensions
{
    /// <summary>
    /// Creates a MeterProvider with safe defaults.
    /// </summary>
    public static MeterProvider CreateMeterProvider(string meterName, Action<MetricsOptions>? configure = null)
    {
        var options = new MetricsOptions();
        configure?.Invoke(options);

        var builder = Sdk.CreateMeterProviderBuilder()
            .AddMeter(meterName)
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(options.ServiceName, serviceVersion: options.ServiceVersion));

        // Add custom resource attributes
        if (options.ResourceAttributes.Count > 0)
        {
            builder.SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(options.ServiceName, serviceVersion: options.ServiceVersion)
                    .AddAttributes(options.ResourceAttributes.Select(kv => new KeyValuePair<string, object>(kv.Key, kv.Value))));
        }

        if (options.ExportToConsole)
        {
            builder.AddConsoleExporter();
        }

        return builder.Build()!;
    }

    /// <summary>
    /// Creates a Meter with the specified name.
    /// </summary>
    public static Meter CreateMeter(string meterName, string? version = null)
    {
        return new Meter(meterName, version);
    }

    /// <summary>
    /// Creates an IMetricsRecorder with OpenTelemetry and safe defaults.
    /// </summary>
    public static IMetricsRecorder CreateMetricsRecorder(
        string meterName, 
        Action<MetricsOptions>? configure = null,
        MeterProvider? meterProvider = null)
    {
        // Create meter provider if not provided
        if (meterProvider == null && configure != null)
        {
            meterProvider = CreateMeterProvider(meterName, configure);
        }

        var meter = CreateMeter(meterName);
        return new OpenTelemetryMetricsRecorder(meter);
    }

    /// <summary>
    /// Wraps an existing Meter as an IMetricsRecorder.
    /// </summary>
    public static IMetricsRecorder ToRecorder(this Meter meter)
    {
        return new OpenTelemetryMetricsRecorder(meter);
    }
}
