using NoWa.Common.Logging;

namespace NoWa.Test.Shared;

/// <summary>
/// Dummy logger for unit testing.
/// Saves a list of logged messages and their levels.
/// </summary>
public class TestLogger : ILogger
{
    /// <summary>
    /// The list of all log levels for every logged message.
    /// </summary>
    public IList<LogLevel> LogLevels { get; } = new List<LogLevel>();

    /// <summary>
    /// The list of all logged messages.
    /// </summary>
    public IList<string> Messages { get; } = new List<string>();


    /// <inheritdoc/>
    /// <remarks>
    /// The <see cref="DisplayLevel"/> is not used at all by the <see cref="TestLogger"/>.
    /// </remarks>
    public LogLevel DisplayLevel { get; set; }

    /// <summary>
    /// Add a message to the logger's lists.
    /// </summary>
    /// <inheritdoc/>
    public void Log(LogLevel level, string message)
    {
        LogLevels.Add(level);
        Messages.Add(message);
    }
}
