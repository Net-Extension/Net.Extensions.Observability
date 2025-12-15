namespace Net.Extensions.Observability.Metrics;

/// <summary>
/// Configuration options for OpenTelemetry metrics.
/// </summary>
public class MetricsOptions
{
    /// <summary>
    /// Gets or sets the service name for metrics. Default is "MyService".
    /// </summary>
    public string ServiceName { get; set; } = "MyService";

    /// <summary>
    /// Gets or sets the service version for metrics. Default is "1.0.0".
    /// </summary>
    public string ServiceVersion { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets whether to export metrics to console. Default is false.
    /// </summary>
    public bool ExportToConsole { get; set; } = false;

    /// <summary>
    /// Gets or sets the console export interval in milliseconds. Default is 10000 (10 seconds).
    /// </summary>
    public int ConsoleExportIntervalMilliseconds { get; set; } = 10000;

    /// <summary>
    /// Gets or sets additional resource attributes.
    /// </summary>
    public Dictionary<string, object> ResourceAttributes { get; set; } = new();
}
