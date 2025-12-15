namespace Net.Extensions.Observability.Logging;

/// <summary>
/// Configuration options for Serilog logging.
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// Gets or sets the minimum log level. Default is Information.
    /// </summary>
    public string MinimumLevel { get; set; } = "Information";

    /// <summary>
    /// Gets or sets whether to write to console. Default is true.
    /// </summary>
    public bool WriteToConsole { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to write to file. Default is false.
    /// </summary>
    public bool WriteToFile { get; set; } = false;

    /// <summary>
    /// Gets or sets the file path for file logging. Default is "logs/app-.log".
    /// </summary>
    public string FilePath { get; set; } = "logs/app-.log";

    /// <summary>
    /// Gets or sets the rolling interval for file logging. Default is Day.
    /// </summary>
    public string RollingInterval { get; set; } = "Day";

    /// <summary>
    /// Gets or sets the maximum file size in bytes. Default is 100MB.
    /// </summary>
    public long? FileSizeLimitBytes { get; set; } = 100 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the number of retained files. Default is 31.
    /// </summary>
    public int? RetainedFileCount { get; set; } = 31;

    /// <summary>
    /// Gets or sets whether to enrich logs with environment information. Default is true.
    /// </summary>
    public bool EnrichWithEnvironment { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to enrich logs with thread information. Default is true.
    /// </summary>
    public bool EnrichWithThread { get; set; } = true;

    /// <summary>
    /// Gets or sets the output template for console logging.
    /// </summary>
    public string? ConsoleOutputTemplate { get; set; }

    /// <summary>
    /// Gets or sets the output template for file logging.
    /// </summary>
    public string? FileOutputTemplate { get; set; }
}
