using System.Diagnostics;
using Net.Extensions.Observability.Core.Tracing;

namespace Net.Extensions.Observability.Tracing;

/// <summary>
/// OpenTelemetry-based implementation of ISpanContext.
/// </summary>
public sealed class OpenTelemetrySpanContext : ISpanContext
{
    private readonly Activity _activity;

    /// <summary>
    /// Initializes a new instance of OpenTelemetrySpanContext.
    /// </summary>
    public OpenTelemetrySpanContext(Activity activity)
    {
        _activity = activity ?? throw new ArgumentNullException(nameof(activity));
    }

    /// <inheritdoc />
    public string TraceId => _activity.TraceId.ToString();

    /// <inheritdoc />
    public string SpanId => _activity.SpanId.ToString();

    /// <inheritdoc />
    public byte TraceFlags => (byte)_activity.ActivityTraceFlags;
}
