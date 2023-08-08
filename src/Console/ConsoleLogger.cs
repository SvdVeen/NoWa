using NoWa.Common.Logging;
using Con = System.Console;

namespace NoWa.Console;

/// <summary>
/// Implementation of an <see cref="ILogger"/> that logs messages to the console.
/// </summary>
internal class ConsoleLogger : ILogger
{
    /// <inheritdoc/>
    public LogLevel DisplayLevel { get; set; } = LogLevel.Info;

    /// <summary>
    /// Log a message to the console with the given <see cref="LogLevel"/>.
    /// </summary>
    /// <exception cref="ArgumentException">A message of <see cref="LogLevel.None"/> was logged.</exception>
    /// <inheritdoc/>
    public void Log(LogLevel level, string message)
    {
        if (level == LogLevel.None)
        {
            throw new ArgumentException("Cannot show messages of level None.", nameof(level));
        }
        if (level <= DisplayLevel)
        {
            if (level == LogLevel.Error)
            {
                Con.Error.WriteLine(message);
            }
            else
            {
                 Con.WriteLine(message);
            }
        }
    }
}
