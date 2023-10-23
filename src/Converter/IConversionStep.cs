using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// Interface for conversion steps.
/// </summary>
public interface IConversionStep<TGrammar>
    where TGrammar : CFG
{
    /// <summary>
    /// The <see cref="ILogger"/> used in this conversion step.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Performs the conversion step on the given grammar.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public void Convert(TGrammar grammar);
}
