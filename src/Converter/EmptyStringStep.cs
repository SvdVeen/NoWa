using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates all ε-productions in a grammar.
/// </summary>
public sealed class EmptyStringStep : BaseConversionStep
{
    /// <inheritdoc/>
    public EmptyStringStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminates all empty string productions in the given <see cref="CFG"/>.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(CFG grammar)
    {
        Logger.LogInfo("Eliminating ε-productions...");
        GrammarStats stats = new(grammar);

        ISet<Nonterminal> nullables = GetNullableSymbols(grammar);

        RemoveEmptyProductions(grammar);

        HashSet<Production> uniqueProductions = new(grammar.Productions);
        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            Production production = grammar.Productions[i];
            for (int j = 0; j < production.Body.Count; j++)
            {
                if (nullables.Contains(production.Body[j]))
                {
                    Production newProduction = new(production.Head, production.Body);
                    newProduction.Body.RemoveAt(j);
                    if (newProduction.Body.Count > 0 && uniqueProductions.Add(newProduction))
                    {
                        grammar.AddProduction(newProduction);
                    }
                }
            }
        }

        Logger.LogInfo("Eliminated ε-productions.");
        stats.LogDiff(grammar, Logger);
    }

    /// <summary>
    /// Gets the set of all nullable symbols in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to get the nullable symbols in.</param>
    /// <returns>The set of all nullable symbols in the grammar.</returns>
    private ISet<Nonterminal> GetNullableSymbols(CFG grammar)
    {
        HashSet<Nonterminal> nullableSymbols = new();
        // Base step: if a production A --> ε exists in the grammar, A is nullable.
        foreach (Nonterminal nonterminal in grammar.Nonterminals)
        {
            foreach (Production production in grammar.GetProductionsByHead(nonterminal))
            {
                if (production.Body.Count == 1 && production.Body[0] is EmptyString)
                {
                    Logger.LogDebug($"Found nullable symbol: {nonterminal}.");
                    nullableSymbols.Add(nonterminal);
                    break;
                }
            }
        }

        // Induction step: if a production A --> X1 X2... Xn exists, where all X are nullable, A is nullable.
        int lastCount;
        do
        {
            lastCount = nullableSymbols.Count;

            foreach (Nonterminal nonterminal in grammar.Nonterminals)
            {
                if (nullableSymbols.Contains(nonterminal))
                {
                    continue;
                }

                foreach (Production production in grammar.GetProductionsByHead(nonterminal))
                {
                    bool allNullable = true;
                    foreach (ISymbol symbol in production.Body)
                    {
                        if (symbol is not Nonterminal nt || !nullableSymbols.Contains(nt))
                        {
                            allNullable = false;
                            break;
                        }
                    }
                    if (allNullable)
                    {
                        Logger.LogDebug($"Found nullable symbol: {nonterminal}.");
                        nullableSymbols.Add(nonterminal);
                        break;
                    }
                }
            }
        } while (lastCount != nullableSymbols.Count);

        return nullableSymbols;
    }

    /// <summary>
    /// Removes all direct ε-productions.
    /// </summary>
    /// <param name="grammar">The grammar to remove the ε-productions from.</param>
    private void RemoveEmptyProductions(CFG grammar)
    {
        List<Production> originalProductions = new(grammar.Productions);

        grammar.Clear();

        foreach (Production production in originalProductions)
        {
            if (production.Body.Count > 1 || production.Body[0] is not EmptyString)
            {
                grammar.AddProduction(production);
            }
            else
            {
                Logger.LogDebug($"Omitting production {production}.");
            }
        }
    }
}
