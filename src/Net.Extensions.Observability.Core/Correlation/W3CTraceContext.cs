namespace Net.Extensions.Observability.Core.Correlation;

/// <summary>
/// W3C Trace Context header names.
/// </summary>
public static class W3CTraceContext
{
    /// <summary>
    /// The traceparent header name.
    /// </summary>
    public const string TraceParentHeader = "traceparent";

    /// <summary>
    /// The tracestate header name.
    /// </summary>
    public const string TraceStateHeader = "tracestate";

    /// <summary>
    /// Parses a traceparent header value.
    /// </summary>
    /// <param name="traceparent">The traceparent header value (format: version-traceId-spanId-flags).</param>
    /// <returns>A tuple containing version, traceId, spanId, and flags.</returns>
    public static (byte version, string traceId, string spanId, byte flags)? ParseTraceParent(string? traceparent)
    {
        if (string.IsNullOrWhiteSpace(traceparent))
            return null;

        var parts = traceparent.Split('-');
        if (parts.Length != 4)
            return null;

        if (!byte.TryParse(parts[0], System.Globalization.NumberStyles.HexNumber, null, out var version))
            return null;

        var traceId = parts[1];
        var spanId = parts[2];

        if (!byte.TryParse(parts[3], System.Globalization.NumberStyles.HexNumber, null, out var flags))
            return null;

        // Validate trace ID and span ID lengths
        if (traceId.Length != 32 || spanId.Length != 16)
            return null;

        return (version, traceId, spanId, flags);
    }

    /// <summary>
    /// Creates a traceparent header value.
    /// </summary>
    public static string CreateTraceParent(string traceId, string spanId, byte flags = 0)
    {
        return $"00-{traceId}-{spanId}-{flags:x2}";
    }

    /// <summary>
    /// Generates a new random trace ID (32 hex characters).
    /// </summary>
    public static string GenerateTraceId()
    {
        var bytes = new byte[16];
        Random.Shared.NextBytes(bytes);
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    /// <summary>
    /// Generates a new random span ID (16 hex characters).
    /// </summary>
    public static string GenerateSpanId()
    {
        var bytes = new byte[8];
        Random.Shared.NextBytes(bytes);
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }
}
