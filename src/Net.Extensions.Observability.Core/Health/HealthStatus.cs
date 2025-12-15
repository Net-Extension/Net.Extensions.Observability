namespace Net.Extensions.Observability.Core.Health;

/// <summary>
/// Defines the status of a health check.
/// </summary>
public enum HealthStatus
{
    /// <summary>
    /// Indicates that the component is unhealthy.
    /// </summary>
    Unhealthy = 0,

    /// <summary>
    /// Indicates that the component is in a degraded state.
    /// </summary>
    Degraded = 1,

    /// <summary>
    /// Indicates that the component is healthy.
    /// </summary>
    Healthy = 2
}
