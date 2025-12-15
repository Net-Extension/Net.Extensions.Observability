using System.Diagnostics;
using System.Diagnostics.Metrics;
using Net.Extensions.Observability.Core.Metrics;

namespace Net.Extensions.Observability.Metrics;

/// <summary>
/// OpenTelemetry-based implementation of IMetricsRecorder.
/// </summary>
public sealed class OpenTelemetryMetricsRecorder : IMetricsRecorder
{
    private readonly Meter _meter;

    /// <summary>
    /// Initializes a new instance of OpenTelemetryMetricsRecorder.
    /// </summary>
    public OpenTelemetryMetricsRecorder(Meter meter)
    {
        _meter = meter ?? throw new ArgumentNullException(nameof(meter));
    }

    /// <inheritdoc />
    public void RecordCounter(string name, long value, params KeyValuePair<string, object?>[] tags)
    {
        var counter = _meter.CreateCounter<long>(name);
        if (tags.Length > 0)
        {
            counter.Add(value, tags);
        }
        else
        {
            counter.Add(value);
        }
    }

    /// <inheritdoc />
    public void IncrementCounter(string name, params KeyValuePair<string, object?>[] tags)
    {
        RecordCounter(name, 1, tags);
    }

    /// <inheritdoc />
    public void RecordGauge(string name, double value, params KeyValuePair<string, object?>[] tags)
    {
        // ObservableGauge requires a callback, so we use Histogram for instant values
        // For true gauge behavior, users should create ObservableGauge directly
        var histogram = _meter.CreateHistogram<double>(name);
        if (tags.Length > 0)
        {
            histogram.Record(value, tags);
        }
        else
        {
            histogram.Record(value);
        }
    }

    /// <inheritdoc />
    public void RecordHistogram(string name, double value, params KeyValuePair<string, object?>[] tags)
    {
        var histogram = _meter.CreateHistogram<double>(name);
        if (tags.Length > 0)
        {
            histogram.Record(value, tags);
        }
        else
        {
            histogram.Record(value);
        }
    }

    /// <inheritdoc />
    public void RecordTimer(string name, long milliseconds, params KeyValuePair<string, object?>[] tags)
    {
        var histogram = _meter.CreateHistogram<long>(name, unit: "ms");
        if (tags.Length > 0)
        {
            histogram.Record(milliseconds, tags);
        }
        else
        {
            histogram.Record(milliseconds);
        }
    }

    /// <inheritdoc />
    public IDisposable StartTimer(string name, params KeyValuePair<string, object?>[] tags)
    {
        return new TimerScope(this, name, tags);
    }

    private sealed class TimerScope : IDisposable
    {
        private readonly OpenTelemetryMetricsRecorder _recorder;
        private readonly string _name;
        private readonly KeyValuePair<string, object?>[] _tags;
        private readonly long _startTicks;

        public TimerScope(OpenTelemetryMetricsRecorder recorder, string name, KeyValuePair<string, object?>[] tags)
        {
            _recorder = recorder;
            _name = name;
            _tags = tags;
            _startTicks = Stopwatch.GetTimestamp();
        }

        public void Dispose()
        {
            var elapsedTicks = Stopwatch.GetTimestamp() - _startTicks;
            var elapsedMs = (long)(elapsedTicks * 1000.0 / Stopwatch.Frequency);
            _recorder.RecordTimer(_name, elapsedMs, _tags);
        }
    }
}
