using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that separates all terminals into their own productions.
/// </summary>
public sealed class SeparateTerminalsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public SeparateTerminalsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Separates all terminals in productions of more than one symbol into their own productions.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        throw new NotImplementedException();
    }
}
