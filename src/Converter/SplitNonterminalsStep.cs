using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that splits productions with more than two nonterminals into multiple substeps.
/// </summary>
public sealed class SplitNonterminalsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public SplitNonterminalsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Splits all productions with more than two nonterminals into multiple substeps.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        throw new NotImplementedException();
    }
}
