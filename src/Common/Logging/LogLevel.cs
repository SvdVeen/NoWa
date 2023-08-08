namespace NoWa.Common.Logging;

/// <summary>
/// A log level for an <see cref="ILogger"/>.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Level for no messages (display only).
    /// </summary>
    None = -1,

    /// <summary>
    /// Level for error messages.
    /// </summary>
    Error = 0,

    /// <summary>
    /// Level for warning messages.
    /// </summary>
    Warning,

    /// <summary>
    /// Level for informational messages.
    /// </summary>
    Info,

    /// <summary>
    /// Level for debug messages.
    /// </summary>
    Debug
}
