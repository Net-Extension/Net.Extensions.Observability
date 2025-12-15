namespace Net.Extensions.Observability.Core.Health;

/// <summary>
/// A no-operation health check implementation that always returns healthy.
/// </summary>
public sealed class NoopHealthCheck : IHealthCheck
{
    /// <summary>
    /// Gets the singleton instance of the NoopHealthCheck.
    /// </summary>
    public static readonly NoopHealthCheck Instance = new();

    private NoopHealthCheck() { }

    /// <inheritdoc />
    public string Name => "Noop";

    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(HealthCheckResult.Healthy("Noop health check always returns healthy"));
}
