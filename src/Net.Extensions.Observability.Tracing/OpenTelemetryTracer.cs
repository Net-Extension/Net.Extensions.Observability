using System.Diagnostics;
using Net.Extensions.Observability.Core.Tracing;
using OpenTelemetry.Trace;

namespace Net.Extensions.Observability.Tracing;

/// <summary>
/// OpenTelemetry-based implementation of ITracer.
/// </summary>
public sealed class OpenTelemetryTracer : ITracer
{
    private readonly ActivitySource _activitySource;

    /// <summary>
    /// Initializes a new instance of OpenTelemetryTracer.
    /// </summary>
    public OpenTelemetryTracer(ActivitySource activitySource)
    {
        _activitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    /// <inheritdoc />
    public ISpan StartSpan(string operationName, Core.Tracing.SpanKind kind = Core.Tracing.SpanKind.Internal)
    {
        var activity = _activitySource.StartActivity(operationName, ConvertActivityKind(kind));
        return activity != null ? new OpenTelemetrySpan(activity) : Core.Tracing.NoopSpan.Instance;
    }

    /// <inheritdoc />
    public ISpan StartChildSpan(string operationName, Core.Tracing.SpanKind kind = Core.Tracing.SpanKind.Internal)
    {
        var activity = _activitySource.StartActivity(operationName, ConvertActivityKind(kind));
        return activity != null ? new OpenTelemetrySpan(activity) : Core.Tracing.NoopSpan.Instance;
    }

    /// <inheritdoc />
    public ISpan? CurrentSpan
    {
        get
        {
            var current = Activity.Current;
            return current != null ? new OpenTelemetrySpan(current) : null;
        }
    }

    private static ActivityKind ConvertActivityKind(Core.Tracing.SpanKind kind) => kind switch
    {
        Core.Tracing.SpanKind.Internal => ActivityKind.Internal,
        Core.Tracing.SpanKind.Server => ActivityKind.Server,
        Core.Tracing.SpanKind.Client => ActivityKind.Client,
        Core.Tracing.SpanKind.Producer => ActivityKind.Producer,
        Core.Tracing.SpanKind.Consumer => ActivityKind.Consumer,
        _ => ActivityKind.Internal
    };
}
