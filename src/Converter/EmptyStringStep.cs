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
    /// Eliminates all empty string productions in the given <see cref="Grammar"/>.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        Logger.LogInfo("Eliminating ε-productions...");
        GrammarStats stats = new(grammar);

        if (grammar.Productions.Count > 0 )
        {
            EliminateEmptyStringProductions(grammar);
        }

        Logger.LogInfo("Eliminated ε-productions.");
        stats.LogDiff(grammar, Logger);
    }

    /// <summary>
    /// Eliminates empty string productions in a given <see cref="Grammar"/>.
    /// </summary>
    /// <param name="grammar"></param>
    private void EliminateEmptyStringProductions(Grammar grammar)
    {
        ISet<Nonterminal> nullables = GetNullableSymbols(grammar);

        RemoveEmptyProductions(grammar);
        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            Production production = grammar.Productions[i];
            for (int j = 0; j < production.Body.Count; j++)
            {
                if (nullables.Contains(production.Body[j]))
                {
                    Production newProduction = production.Clone();
                    newProduction.Body.RemoveAt(j);
                    if (newProduction.Body.Count > 0 && !grammar.Productions.Contains(newProduction))
                    {
                        grammar.AddProduction(newProduction);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the set of all nullable symbols in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to get the nullable symbols in.</param>
    /// <returns>The set of all nullable symbols in the grammar.</returns>
    private ISet<Nonterminal> GetNullableSymbols(Grammar grammar)
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
    private void RemoveEmptyProductions(Grammar grammar)
    {
        Grammar originalGrammar = grammar.Clone();
        grammar.Clear();
        grammar.StartSymbol = originalGrammar.StartSymbol;

        for (int i = 0; i < originalGrammar.Productions.Count; i++)
        {
            Production production = originalGrammar.Productions[i];
            if (production.Body.Count > 1 || production.Body[0] is not EmptyString || production.Head == grammar.StartSymbol)
            {
                grammar.AddProduction(production);
            }
            else
            {
                Logger.LogDebug($"Omitting production {production}.");
            }
        }
        if (grammar is WAG wag)
        {
            WAG originalWag = (WAG)originalGrammar;
            foreach (var nonterminal in grammar.Nonterminals)
            {
                foreach (char inheritedattr in originalWag.GetInheritedAttributes(nonterminal))
                {
                    wag.AddInheritedAttribute(nonterminal, inheritedattr);
                }
                foreach (char synthesizedattr in originalWag.GetSynthesizedAttributes(nonterminal))
                {
                    wag.AddSynthesizedAttribute(nonterminal, synthesizedattr);
                }
                foreach (char staticattr in originalWag.GetStaticAttributes(nonterminal))
                {
                    wag.AddStaticAttribute(nonterminal, staticattr);
                }
            }
        }
    }
}
