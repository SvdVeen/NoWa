using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates any productions with a weight of zero.
/// </summary>
public sealed class ZeroWeightsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public ZeroWeightsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminates all productions with a weight of zero in the grammar.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        Logger.LogInfo("Eliminating zero-weight productions...");
        GrammarStats stats = new(grammar);

        EliminateZeroWeights(grammar);

        Logger.LogInfo("Zero-weight productions eliminated.");
        stats.LogDiff(grammar, Logger);
    }

    /// <summary>
    /// Remove all productions with a weight of zero from the given grammar.
    /// </summary>
    /// <param name="grammar">The grammar to remove productions from.</param>
    private static void EliminateZeroWeights(Grammar grammar)
    {
        if (grammar.Productions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            if (grammar.Productions[i].Weight.GetDouble(out double weight) && weight == 0)
            {
                grammar.RemoveProductionAt(i--);
            }
        }
    }
}
