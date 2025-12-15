using Net.Extensions.Observability.Core.Health;

namespace Net.Extensions.Observability.Health;

/// <summary>
/// Extension methods for health checks.
/// </summary>
public static class HealthExtensions
{
    /// <summary>
    /// Runs multiple health checks and returns an aggregated result.
    /// </summary>
    public static async Task<HealthCheckResult> CheckAllAsync(
        this IEnumerable<IHealthCheck> healthChecks,
        CancellationToken cancellationToken = default,
        HealthOptions? options = null)
    {
        options ??= new HealthOptions();

        var checks = healthChecks.ToList();
        if (checks.Count == 0)
        {
            return HealthCheckResult.Healthy("No health checks configured");
        }

        var results = new Dictionary<string, object>();
        var overallStatus = HealthStatus.Healthy;
        var descriptions = new List<string>();
        Exception? firstException = null;

        foreach (var check in checks)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(options.TimeoutMilliseconds);

                var result = await check.CheckHealthAsync(cts.Token);

                if (options.IncludeDetails)
                {
                    results[check.Name] = new
                    {
                        status = result.Status.ToString(),
                        description = result.Description,
                        exception = options.IncludeExceptions ? result.Exception?.Message : null,
                        data = result.Data
                    };
                }

                // Determine overall status
                if (result.Status < overallStatus)
                {
                    overallStatus = result.Status;
                }

                if (!string.IsNullOrWhiteSpace(result.Description))
                {
                    descriptions.Add($"{check.Name}: {result.Description}");
                }

                if (result.Exception != null && firstException == null)
                {
                    firstException = result.Exception;
                }
            }
            catch (Exception ex)
            {
                overallStatus = HealthStatus.Unhealthy;
                descriptions.Add($"{check.Name}: {ex.Message}");
                
                if (firstException == null)
                {
                    firstException = ex;
                }

                if (options.IncludeDetails)
                {
                    results[check.Name] = new
                    {
                        status = "Unhealthy",
                        description = "Health check threw an exception",
                        exception = options.IncludeExceptions ? ex.Message : null
                    };
                }
            }
        }

        var description = descriptions.Count > 0 ? string.Join("; ", descriptions) : null;

        return new HealthCheckResult
        {
            Status = overallStatus,
            Description = description,
            Data = results,
            Exception = options.IncludeExceptions ? firstException : null
        };
    }

    /// <summary>
    /// Creates a single aggregated health check from multiple health checks.
    /// </summary>
    public static IHealthCheck Aggregate(string name, params IHealthCheck[] healthChecks) =>
        new AggregatedHealthCheck(name, healthChecks);

    private sealed class AggregatedHealthCheck : IHealthCheck
    {
        private readonly string _name;
        private readonly IHealthCheck[] _healthChecks;

        public AggregatedHealthCheck(string name, IHealthCheck[] healthChecks)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _healthChecks = healthChecks ?? throw new ArgumentNullException(nameof(healthChecks));
        }

        public string Name => _name;

        public Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
        {
            return _healthChecks.CheckAllAsync(cancellationToken);
        }
    }
}
