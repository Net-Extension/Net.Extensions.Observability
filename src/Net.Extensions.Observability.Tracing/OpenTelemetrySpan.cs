using System.Diagnostics;
using Net.Extensions.Observability.Core.Tracing;

namespace Net.Extensions.Observability.Tracing;

/// <summary>
/// OpenTelemetry-based implementation of ISpan.
/// </summary>
public sealed class OpenTelemetrySpan : ISpan
{
    private readonly Activity _activity;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of OpenTelemetrySpan.
    /// </summary>
    public OpenTelemetrySpan(Activity activity)
    {
        _activity = activity ?? throw new ArgumentNullException(nameof(activity));
    }

    /// <inheritdoc />
    public void SetAttribute(string key, object? value)
    {
        if (_disposed) return;
        _activity.SetTag(key, value);
    }

    /// <inheritdoc />
    public void AddEvent(string name, params KeyValuePair<string, object?>[] attributes)
    {
        if (_disposed) return;
        
        if (attributes.Length > 0)
        {
            var activityTags = new ActivityTagsCollection();
            foreach (var attr in attributes)
            {
                activityTags[attr.Key] = attr.Value;
            }
            _activity.AddEvent(new ActivityEvent(name, tags: activityTags));
        }
        else
        {
            _activity.AddEvent(new ActivityEvent(name));
        }
    }

    /// <inheritdoc />
    public void SetStatus(SpanStatus status, string? description = null)
    {
        if (_disposed) return;
        
        var activityStatus = status switch
        {
            SpanStatus.Ok => ActivityStatusCode.Ok,
            SpanStatus.Error => ActivityStatusCode.Error,
            _ => ActivityStatusCode.Unset
        };
        
        _activity.SetStatus(activityStatus, description);
    }

    /// <inheritdoc />
    public void RecordException(Exception exception)
    {
        if (_disposed) return;
        
        var tags = new ActivityTagsCollection
        {
            ["exception.type"] = exception.GetType().FullName,
            ["exception.message"] = exception.Message,
            ["exception.stacktrace"] = exception.StackTrace
        };
        
        _activity.AddEvent(new ActivityEvent("exception", tags: tags));
    }

    /// <inheritdoc />
    public ISpanContext Context => new OpenTelemetrySpanContext(_activity);

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _activity.Dispose();
    }
}
