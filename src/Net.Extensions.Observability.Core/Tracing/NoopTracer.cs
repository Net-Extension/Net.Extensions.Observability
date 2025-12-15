namespace Net.Extensions.Observability.Core.Tracing;

/// <summary>
/// A no-operation tracer implementation that does nothing.
/// </summary>
public sealed class NoopTracer : ITracer
{
    /// <summary>
    /// Gets the singleton instance of the NoopTracer.
    /// </summary>
    public static readonly NoopTracer Instance = new();

    private NoopTracer() { }

    /// <inheritdoc />
    public ISpan StartSpan(string operationName, SpanKind kind = SpanKind.Internal) => NoopSpan.Instance;

    /// <inheritdoc />
    public ISpan StartChildSpan(string operationName, SpanKind kind = SpanKind.Internal) => NoopSpan.Instance;

    /// <inheritdoc />
    public ISpan? CurrentSpan => null;
}

/// <summary>
/// A no-operation span implementation that does nothing.
/// </summary>
public sealed class NoopSpan : ISpan
{
    /// <summary>
    /// Gets the singleton instance of the NoopSpan.
    /// </summary>
    public static readonly NoopSpan Instance = new();

    private NoopSpan() { }

    /// <inheritdoc />
    public void SetAttribute(string key, object? value) { }

    /// <inheritdoc />
    public void AddEvent(string name, params KeyValuePair<string, object?>[] attributes) { }

    /// <inheritdoc />
    public void SetStatus(SpanStatus status, string? description = null) { }

    /// <inheritdoc />
    public void RecordException(Exception exception) { }

    /// <inheritdoc />
    public ISpanContext Context => NoopSpanContext.Instance;

    /// <inheritdoc />
    public void Dispose() { }
}

/// <summary>
/// A no-operation span context implementation.
/// </summary>
public sealed class NoopSpanContext : ISpanContext
{
    /// <summary>
    /// Gets the singleton instance of the NoopSpanContext.
    /// </summary>
    public static readonly NoopSpanContext Instance = new();

    private NoopSpanContext() { }

    /// <inheritdoc />
    public string TraceId => "00000000000000000000000000000000";

    /// <inheritdoc />
    public string SpanId => "0000000000000000";

    /// <inheritdoc />
    public byte TraceFlags => 0;
}
