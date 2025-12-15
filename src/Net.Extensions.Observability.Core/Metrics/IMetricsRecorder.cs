namespace Net.Extensions.Observability.Core.Metrics;

/// <summary>
/// Defines a generic metrics recorder interface.
/// </summary>
public interface IMetricsRecorder
{
    /// <summary>
    /// Records a counter metric.
    /// </summary>
    void RecordCounter(string name, long value, params KeyValuePair<string, object?>[] tags);

    /// <summary>
    /// Increments a counter metric by 1.
    /// </summary>
    void IncrementCounter(string name, params KeyValuePair<string, object?>[] tags);

    /// <summary>
    /// Records a gauge metric.
    /// </summary>
    void RecordGauge(string name, double value, params KeyValuePair<string, object?>[] tags);

    /// <summary>
    /// Records a histogram metric.
    /// </summary>
    void RecordHistogram(string name, double value, params KeyValuePair<string, object?>[] tags);

    /// <summary>
    /// Records a timer metric in milliseconds.
    /// </summary>
    void RecordTimer(string name, long milliseconds, params KeyValuePair<string, object?>[] tags);

    /// <summary>
    /// Creates a timer scope that automatically records elapsed time when disposed.
    /// </summary>
    IDisposable StartTimer(string name, params KeyValuePair<string, object?>[] tags);
}
