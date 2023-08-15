namespace NoWa.Common.Logging;

/// <summary>
/// Interface for loggers used in conversion.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// The current display log level of the logger.
    /// </summary>
    public LogLevel DisplayLevel { get; set; }

    /// <summary>
    /// Log a message using the given log level.
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/> to display the message at.</param>
    /// <param name="message">The message to log.</param>
    public void Log(LogLevel level, string message);

    /// <summary>
    /// Log a debug message using the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogDebug(string message) => Log(LogLevel.Debug, message);

    /// <summary>
    /// Log an informational message using the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogInfo(string message) => Log(LogLevel.Info, message);

    /// <summary>
    /// Log a warning message using the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogWarning(string message) => Log(LogLevel.Warning, message);

    /// <summary>
    /// Log an error message using the logger.
    /// </summary>
    /// <param name="message">The error to log.</param>
    public void LogError(string message) => Log(LogLevel.Error, message);
}
