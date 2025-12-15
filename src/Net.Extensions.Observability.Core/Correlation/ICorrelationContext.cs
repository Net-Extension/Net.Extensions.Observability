namespace Net.Extensions.Observability.Core.Correlation;

/// <summary>
/// Provides W3C Trace Context correlation support.
/// </summary>
public interface ICorrelationContext
{
    /// <summary>
    /// Gets the current trace ID.
    /// </summary>
    string? TraceId { get; }

    /// <summary>
    /// Gets the current span ID.
    /// </summary>
    string? SpanId { get; }

    /// <summary>
    /// Gets the current trace flags.
    /// </summary>
    byte TraceFlags { get; }

    /// <summary>
    /// Gets the current trace state.
    /// </summary>
    string? TraceState { get; }

    /// <summary>
    /// Sets correlation information for the current context.
    /// </summary>
    void SetCorrelation(string traceId, string spanId, byte traceFlags = 0, string? traceState = null);

    /// <summary>
    /// Clears correlation information from the current context.
    /// </summary>
    void ClearCorrelation();
}
