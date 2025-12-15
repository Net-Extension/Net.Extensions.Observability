namespace Net.Extensions.Observability.Core.Correlation;

/// <summary>
/// A no-operation correlation context implementation.
/// </summary>
public sealed class NoopCorrelationContext : ICorrelationContext
{
    /// <summary>
    /// Gets the singleton instance of the NoopCorrelationContext.
    /// </summary>
    public static readonly NoopCorrelationContext Instance = new();

    private NoopCorrelationContext() { }

    /// <inheritdoc />
    public string? TraceId => null;

    /// <inheritdoc />
    public string? SpanId => null;

    /// <inheritdoc />
    public byte TraceFlags => 0;

    /// <inheritdoc />
    public string? TraceState => null;

    /// <inheritdoc />
    public void SetCorrelation(string traceId, string spanId, byte traceFlags = 0, string? traceState = null) { }

    /// <inheritdoc />
    public void ClearCorrelation() { }
}
