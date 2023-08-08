using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// Interface for conversion steps.
/// </summary>
public interface IConversionStep
{
    /// <summary>
    /// The <see cref="ILogger"/> used in this conversion step.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Performs the conversion step on the given <see cref="Grammar"/>.
    /// </summary>
    /// <param name="grammar">The <see cref="Grammar"/> to convert.</param>
    public void Convert(Grammar grammar);
}
