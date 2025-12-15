namespace Net.Extensions.Observability.Health;

/// <summary>
/// Configuration options for health checks.
/// </summary>
public class HealthOptions
{
    /// <summary>
    /// Gets or sets the timeout for each health check in milliseconds. Default is 5000 (5 seconds).
    /// </summary>
    public int TimeoutMilliseconds { get; set; } = 5000;

    /// <summary>
    /// Gets or sets whether to include detailed error information. Default is true.
    /// </summary>
    public bool IncludeDetails { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to include exception details. Default is false.
    /// </summary>
    public bool IncludeExceptions { get; set; } = false;
}
