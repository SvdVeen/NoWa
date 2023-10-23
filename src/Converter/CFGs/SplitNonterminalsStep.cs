using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter.CFGs;

/// <summary>
/// A conversion step that splits productions with more than two nonterminals into multiple substeps.
/// </summary>
public sealed class SplitNonterminalsStep : BaseConversionStep<CFG>
{
    /// <inheritdoc/>
    public SplitNonterminalsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Splits all productions with more than two nonterminals into multiple substeps.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(CFG grammar)
    {
        Logger.LogInfo("Splitting production bodies longer than 2...");
        GrammarStats stats = new(grammar);

        Dictionary<string, Nonterminal> newNonterminals = new();
        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            SplitProduction(grammar, newNonterminals, grammar.Productions[i]);
        }

        Logger.LogInfo("Production bodies longer than 2 split.");
        stats.LogDiff(grammar, Logger);
    }

    /// <summary>
    /// Splits a production into smaller ones if it is longer than 3 symbols.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <param name="newNonterminals">The newly introduced nonterminals in the step.</param>
    /// <param name="production">The production to split.</param>
    private void SplitProduction(CFG grammar, IDictionary<string, Nonterminal> newNonterminals, Production production)
    {
        while (production.Body.Count > 2)
        {
            Logger.LogDebug($"Replacing in production {production}.");
            string newName = $"{production.Body[^2]}-{production.Body[^1]}";
            if (!newNonterminals.TryGetValue(newName, out Nonterminal? newNonterminal))
            {
                Production newProduction = new(Nonterminal.Get(newName), production.Body[^2], production.Body[^1]);
                grammar.AddProduction(newProduction);
                newNonterminal = newProduction.Head;
                newNonterminals.Add(newName, newNonterminal);
                Logger.LogDebug($"Added production {newProduction}");
            }
            production.Body.RemoveAt(production.Body.Count - 1);
            production.Body.RemoveAt(production.Body.Count - 1);
            production.Body.Add(newNonterminal);
            Logger.LogDebug($"Updated production: {production}");
        }
    }
}
