using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates any unreachable symbols.
/// </summary>
public sealed class UnreachableSymbolsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public UnreachableSymbolsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminates all unreachable ('useless') symbols from the given grammar.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        throw new NotImplementedException();
    }
}
