namespace Net.Extensions.Observability.Tracing;

/// <summary>
/// Configuration options for OpenTelemetry tracing.
/// </summary>
public class TracingOptions
{
    /// <summary>
    /// Gets or sets the service name for tracing. Default is "MyService".
    /// </summary>
    public string ServiceName { get; set; } = "MyService";

    /// <summary>
    /// Gets or sets the service version for tracing. Default is "1.0.0".
    /// </summary>
    public string ServiceVersion { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets whether to export traces to console. Default is false.
    /// </summary>
    public bool ExportToConsole { get; set; } = false;

    /// <summary>
    /// Gets or sets the sampling ratio (0.0 to 1.0). Default is 1.0 (100% sampling).
    /// </summary>
    public double SamplingRatio { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets whether to enable W3C trace context propagation. Default is true.
    /// </summary>
    public bool EnableW3CTraceContext { get; set; } = true;

    /// <summary>
    /// Gets or sets additional resource attributes.
    /// </summary>
    public Dictionary<string, object> ResourceAttributes { get; set; } = new();
}
