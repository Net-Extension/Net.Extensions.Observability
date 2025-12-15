namespace Net.Extensions.Observability.Core.Health;

/// <summary>
/// Defines a health check interface.
/// </summary>
public interface IHealthCheck
{
    /// <summary>
    /// Executes the health check.
    /// </summary>
    Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the name of the health check.
    /// </summary>
    string Name { get; }
}

/// <summary>
/// Represents the result of a health check.
/// </summary>
public class HealthCheckResult
{
    /// <summary>
    /// Gets the status of the health check.
    /// </summary>
    public HealthStatus Status { get; init; }

    /// <summary>
    /// Gets the description of the health check result.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets additional data associated with the health check.
    /// </summary>
    public IReadOnlyDictionary<string, object>? Data { get; init; }

    /// <summary>
    /// Gets the exception that occurred during the health check, if any.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Creates a healthy result.
    /// </summary>
    public static HealthCheckResult Healthy(string? description = null, IReadOnlyDictionary<string, object>? data = null) =>
        new() { Status = HealthStatus.Healthy, Description = description, Data = data };

    /// <summary>
    /// Creates a degraded result.
    /// </summary>
    public static HealthCheckResult Degraded(string? description = null, Exception? exception = null, IReadOnlyDictionary<string, object>? data = null) =>
        new() { Status = HealthStatus.Degraded, Description = description, Exception = exception, Data = data };

    /// <summary>
    /// Creates an unhealthy result.
    /// </summary>
    public static HealthCheckResult Unhealthy(string? description = null, Exception? exception = null, IReadOnlyDictionary<string, object>? data = null) =>
        new() { Status = HealthStatus.Unhealthy, Description = description, Exception = exception, Data = data };
}
