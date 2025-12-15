namespace Net.Extensions.Observability.Core.Tracing;

/// <summary>
/// Defines a generic tracing interface for distributed tracing.
/// </summary>
public interface ITracer
{
    /// <summary>
    /// Starts a new span with the specified operation name.
    /// </summary>
    ISpan StartSpan(string operationName, SpanKind kind = SpanKind.Internal);

    /// <summary>
    /// Starts a new span as a child of the current active span.
    /// </summary>
    ISpan StartChildSpan(string operationName, SpanKind kind = SpanKind.Internal);

    /// <summary>
    /// Gets the current active span, if any.
    /// </summary>
    ISpan? CurrentSpan { get; }
}

/// <summary>
/// Represents a span in a distributed trace.
/// </summary>
public interface ISpan : IDisposable
{
    /// <summary>
    /// Sets an attribute on the span.
    /// </summary>
    void SetAttribute(string key, object? value);

    /// <summary>
    /// Adds an event to the span.
    /// </summary>
    void AddEvent(string name, params KeyValuePair<string, object?>[] attributes);

    /// <summary>
    /// Sets the status of the span.
    /// </summary>
    void SetStatus(SpanStatus status, string? description = null);

    /// <summary>
    /// Records an exception on the span.
    /// </summary>
    void RecordException(Exception exception);

    /// <summary>
    /// Gets the span context.
    /// </summary>
    ISpanContext Context { get; }
}

/// <summary>
/// Represents the context of a span for correlation.
/// </summary>
public interface ISpanContext
{
    /// <summary>
    /// Gets the trace ID.
    /// </summary>
    string TraceId { get; }

    /// <summary>
    /// Gets the span ID.
    /// </summary>
    string SpanId { get; }

    /// <summary>
    /// Gets the trace flags.
    /// </summary>
    byte TraceFlags { get; }
}
