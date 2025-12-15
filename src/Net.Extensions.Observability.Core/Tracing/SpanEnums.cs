namespace Net.Extensions.Observability.Core.Tracing;

/// <summary>
/// Defines the kind of span.
/// </summary>
public enum SpanKind
{
    /// <summary>
    /// Internal operation.
    /// </summary>
    Internal = 0,

    /// <summary>
    /// Server-side operation.
    /// </summary>
    Server = 1,

    /// <summary>
    /// Client-side operation.
    /// </summary>
    Client = 2,

    /// <summary>
    /// Producer operation.
    /// </summary>
    Producer = 3,

    /// <summary>
    /// Consumer operation.
    /// </summary>
    Consumer = 4
}

/// <summary>
/// Defines the status of a span.
/// </summary>
public enum SpanStatus
{
    /// <summary>
    /// Unset status.
    /// </summary>
    Unset = 0,

    /// <summary>
    /// Success status.
    /// </summary>
    Ok = 1,

    /// <summary>
    /// Error status.
    /// </summary>
    Error = 2
}
