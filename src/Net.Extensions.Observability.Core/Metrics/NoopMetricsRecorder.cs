namespace Net.Extensions.Observability.Core.Metrics;

/// <summary>
/// A no-operation metrics recorder implementation that does nothing.
/// </summary>
public sealed class NoopMetricsRecorder : IMetricsRecorder
{
    /// <summary>
    /// Gets the singleton instance of the NoopMetricsRecorder.
    /// </summary>
    public static readonly NoopMetricsRecorder Instance = new();

    private NoopMetricsRecorder() { }

    /// <inheritdoc />
    public void RecordCounter(string name, long value, params KeyValuePair<string, object?>[] tags) { }

    /// <inheritdoc />
    public void IncrementCounter(string name, params KeyValuePair<string, object?>[] tags) { }

    /// <inheritdoc />
    public void RecordGauge(string name, double value, params KeyValuePair<string, object?>[] tags) { }

    /// <inheritdoc />
    public void RecordHistogram(string name, double value, params KeyValuePair<string, object?>[] tags) { }

    /// <inheritdoc />
    public void RecordTimer(string name, long milliseconds, params KeyValuePair<string, object?>[] tags) { }

    /// <inheritdoc />
    public IDisposable StartTimer(string name, params KeyValuePair<string, object?>[] tags) => NoopDisposable.Instance;

    private sealed class NoopDisposable : IDisposable
    {
        public static readonly NoopDisposable Instance = new();
        private NoopDisposable() { }
        public void Dispose() { }
    }
}
