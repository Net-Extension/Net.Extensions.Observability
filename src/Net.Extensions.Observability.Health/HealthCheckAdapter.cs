using Net.Extensions.Observability.Core.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Net.Extensions.Observability.Health;

/// <summary>
/// Adapter for Microsoft.Extensions.Diagnostics.HealthChecks to IHealthCheck.
/// </summary>
public sealed class HealthCheckAdapter : Core.Health.IHealthCheck
{
    private readonly Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck _healthCheck;
    private readonly string _name;

    /// <summary>
    /// Initializes a new instance of HealthCheckAdapter.
    /// </summary>
    public HealthCheckAdapter(Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck healthCheck, string name)
    {
        _healthCheck = healthCheck ?? throw new ArgumentNullException(nameof(healthCheck));
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public async Task<Core.Health.HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var context = new HealthCheckContext
        {
            Registration = new HealthCheckRegistration(_name, _healthCheck, null, null)
        };

        var result = await _healthCheck.CheckHealthAsync(context, cancellationToken);

        return new Core.Health.HealthCheckResult
        {
            Status = ConvertStatus(result.Status),
            Description = result.Description,
            Exception = result.Exception,
            Data = result.Data
        };
    }

    private static Core.Health.HealthStatus ConvertStatus(Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus status) => status switch
    {
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => Core.Health.HealthStatus.Healthy,
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => Core.Health.HealthStatus.Degraded,
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy => Core.Health.HealthStatus.Unhealthy,
        _ => Core.Health.HealthStatus.Unhealthy
    };
}
