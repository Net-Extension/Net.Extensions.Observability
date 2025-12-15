# Net.Extensions.Observability.Health

Health check implementation for the Net.Extensions.Observability ecosystem. Provides production-ready health monitoring with safe defaults and compatibility with Microsoft.Extensions.Diagnostics.HealthChecks.

## Features

- **Health Check Abstractions**: Implementation of `IHealthCheck` interface
- **Microsoft.Extensions Integration**: Compatible with Microsoft.Extensions.Diagnostics.HealthChecks
- **Basic Health Checks**: Pre-built health checks for common scenarios
- **Aggregated Health Checks**: Combine multiple health checks into one
- **Safe Defaults**: Pre-configured health checks with sensible settings
- **Flexible Configuration**: Easy-to-use options for customizing health check behavior
- **Extension Methods**: Convenient fluent API for health check operations
- **Timeout Support**: Configurable timeouts for health check execution

## Installation

```bash
dotnet add package Net.Extensions.Observability.Health
```

## Quick Start

### Basic Health Checks

```csharp
using Net.Extensions.Observability.Health;

// Create a simple health check
var healthCheck = BasicHealthChecks.AlwaysHealthy("MyService");

// Execute the health check
var result = await healthCheck.CheckHealthAsync();

Console.WriteLine($"Status: {result.Status}");
Console.WriteLine($"Description: {result.Description}");
```

### File and Directory Checks

```csharp
// Check if a file exists
var fileCheck = BasicHealthChecks.FileExists("/app/config/settings.json", "Config File");

// Check if a directory exists
var dirCheck = BasicHealthChecks.DirectoryExists("/app/data", "Data Directory");

var result = await fileCheck.CheckHealthAsync();
```

### Custom Health Checks

```csharp
using Net.Extensions.Observability.Core.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnection _connection;

    public DatabaseHealthCheck(IDbConnection connection)
    {
        _connection = connection;
    }

    public string Name => "Database";

    public async Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _connection.OpenAsync(cancellationToken);
            await _connection.CloseAsync();
            return HealthCheckResult.Healthy("Database connection successful");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failed", ex);
        }
    }
}
```

### Aggregate Multiple Health Checks

```csharp
using Net.Extensions.Observability.Health;

var healthChecks = new[]
{
    BasicHealthChecks.FileExists("/app/config.json", "Config"),
    BasicHealthChecks.DirectoryExists("/app/data", "Data"),
    new DatabaseHealthCheck(dbConnection)
};

// Check all and get aggregated result
var result = await healthChecks.CheckAllAsync();

Console.WriteLine($"Overall Status: {result.Status}");
foreach (var kvp in result.Data ?? new Dictionary<string, object>())
{
    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
}
```

## Configuration Options

The `HealthOptions` class provides the following configuration:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `TimeoutMilliseconds` | int | 5000 | Timeout for each health check (5 seconds) |
| `IncludeDetails` | bool | true | Include detailed information in results |
| `IncludeExceptions` | bool | false | Include exception details in results |

## Advanced Usage

### Custom Health Check with Function

```csharp
var customCheck = BasicHealthChecks.Custom(
    "API Availability",
    async () =>
    {
        try
        {
            var response = await httpClient.GetAsync("/health");
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("API is available");
            }
            return HealthCheckResult.Degraded($"API returned {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("API is unavailable", ex);
        }
    });
```

### Aggregated Health Check

```csharp
var aggregated = HealthExtensions.Aggregate(
    "System Health",
    BasicHealthChecks.FileExists("/app/config.json"),
    BasicHealthChecks.DirectoryExists("/app/data"),
    new DatabaseHealthCheck(dbConnection));

var result = await aggregated.CheckHealthAsync();
```

### Custom Options

```csharp
var options = new HealthOptions
{
    TimeoutMilliseconds = 3000,  // 3 seconds
    IncludeDetails = true,
    IncludeExceptions = true
};

var result = await healthChecks.CheckAllAsync(options: options);
```

## Health Check Results

### Health Status Levels

- **Healthy**: The component is functioning normally
- **Degraded**: The component is functioning but with reduced capabilities
- **Unhealthy**: The component is not functioning

### Result Properties

```csharp
var result = await healthCheck.CheckHealthAsync();

// Access result properties
Console.WriteLine($"Status: {result.Status}");
Console.WriteLine($"Description: {result.Description}");

if (result.Exception != null)
{
    Console.WriteLine($"Exception: {result.Exception.Message}");
}

if (result.Data != null)
{
    foreach (var kvp in result.Data)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}
```

## Integration with Microsoft.Extensions.Diagnostics.HealthChecks

```csharp
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Net.Extensions.Observability.Health;

// Wrap a Microsoft health check
var msHealthCheck = new SomeHealthCheck(); // Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
var adapter = new HealthCheckAdapter(msHealthCheck, "SomeCheck");

var result = await adapter.CheckHealthAsync();
```

## Best Practices

1. **Use Meaningful Names**: Give health checks descriptive names
   ```csharp
   var check = BasicHealthChecks.FileExists("/app/config.json", "Application Configuration");
   ```

2. **Handle Exceptions Gracefully**: Always catch exceptions in custom health checks
   ```csharp
   public async Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken)
   {
       try
       {
           // Perform check
           return HealthCheckResult.Healthy();
       }
       catch (Exception ex)
       {
           return HealthCheckResult.Unhealthy("Check failed", ex);
       }
   }
   ```

3. **Use Appropriate Timeouts**: Configure timeouts based on your environment
   ```csharp
   var options = new HealthOptions { TimeoutMilliseconds = 3000 };
   ```

4. **Return Degraded Status When Appropriate**: Use degraded for non-critical issues
   ```csharp
   if (cacheAvailable)
       return HealthCheckResult.Healthy("Cache is available");
   else
       return HealthCheckResult.Degraded("Cache unavailable, using fallback");
   ```

5. **Include Relevant Data**: Add contextual information to results
   ```csharp
   var data = new Dictionary<string, object>
   {
       ["connectionString"] = "server=localhost",
       ["responseTime"] = responseTimeMs
   };
   return HealthCheckResult.Healthy("Connected", data);
   ```

## Common Health Check Patterns

### HTTP Endpoint Check

```csharp
var httpCheck = BasicHealthChecks.Custom(
    "External API",
    async () =>
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5);
        
        try
        {
            var response = await client.GetAsync("https://api.example.com/health");
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("API is responding")
                : HealthCheckResult.Unhealthy($"API returned {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("API is not responding", ex);
        }
    });
```

### Database Connection Check

```csharp
var dbCheck = BasicHealthChecks.Custom(
    "Database",
    async () =>
    {
        try
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync();
            
            return HealthCheckResult.Healthy("Database is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is not accessible", ex);
        }
    });
```

### Memory Usage Check

```csharp
var memoryCheck = BasicHealthChecks.Custom(
    "Memory",
    () =>
    {
        var gcMemory = GC.GetTotalMemory(false);
        var process = Process.GetCurrentProcess();
        var workingSet = process.WorkingSet64;
        
        var data = new Dictionary<string, object>
        {
            ["gcMemoryMB"] = gcMemory / 1024 / 1024,
            ["workingSetMB"] = workingSet / 1024 / 1024
        };
        
        if (workingSet > 1024 * 1024 * 1024) // 1 GB
        {
            return Task.FromResult(HealthCheckResult.Degraded("High memory usage", data: data));
        }
        
        return Task.FromResult(HealthCheckResult.Healthy("Memory usage is normal", data: data));
    });
```

## Dependencies

- **Net.Extensions.Observability.Core** - Core abstractions
- **Microsoft.Extensions.Diagnostics.HealthChecks** (9.0.0) - Health check abstractions

## Related Packages

- `Net.Extensions.Observability.Core` - Core abstractions and interfaces
- `Net.Extensions.Observability.Logging` - Structured logging
- `Net.Extensions.Observability.Metrics` - Metrics recording
- `Net.Extensions.Observability.Tracing` - Distributed tracing
