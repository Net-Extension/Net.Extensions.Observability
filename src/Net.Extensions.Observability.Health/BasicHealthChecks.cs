using Net.Extensions.Observability.Core.Health;

namespace Net.Extensions.Observability.Health;

/// <summary>
/// Basic health checks for common scenarios.
/// </summary>
public static class BasicHealthChecks
{
    /// <summary>
    /// Creates a simple health check that always returns healthy.
    /// </summary>
    public static IHealthCheck AlwaysHealthy(string name = "Always Healthy") =>
        new SimpleHealthCheck(name, () => Task.FromResult(HealthCheckResult.Healthy("Always returns healthy")));

    /// <summary>
    /// Creates a simple health check that checks if a file exists.
    /// </summary>
    public static IHealthCheck FileExists(string filePath, string? name = null) =>
        new SimpleHealthCheck(
            name ?? $"File: {filePath}",
            () =>
            {
                if (File.Exists(filePath))
                {
                    return Task.FromResult(HealthCheckResult.Healthy($"File exists: {filePath}"));
                }
                return Task.FromResult(HealthCheckResult.Unhealthy($"File does not exist: {filePath}"));
            });

    /// <summary>
    /// Creates a simple health check that checks if a directory exists.
    /// </summary>
    public static IHealthCheck DirectoryExists(string directoryPath, string? name = null) =>
        new SimpleHealthCheck(
            name ?? $"Directory: {directoryPath}",
            () =>
            {
                if (Directory.Exists(directoryPath))
                {
                    return Task.FromResult(HealthCheckResult.Healthy($"Directory exists: {directoryPath}"));
                }
                return Task.FromResult(HealthCheckResult.Unhealthy($"Directory does not exist: {directoryPath}"));
            });

    /// <summary>
    /// Creates a custom health check with a user-provided check function.
    /// </summary>
    public static IHealthCheck Custom(string name, Func<Task<HealthCheckResult>> checkFunc) =>
        new SimpleHealthCheck(name, checkFunc);

    private sealed class SimpleHealthCheck : IHealthCheck
    {
        private readonly string _name;
        private readonly Func<Task<HealthCheckResult>> _checkFunc;

        public SimpleHealthCheck(string name, Func<Task<HealthCheckResult>> checkFunc)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _checkFunc = checkFunc ?? throw new ArgumentNullException(nameof(checkFunc));
        }

        public string Name => _name;

        public Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
        {
            return _checkFunc();
        }
    }
}
