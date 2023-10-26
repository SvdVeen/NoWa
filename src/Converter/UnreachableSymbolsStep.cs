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
        Logger.LogInfo("Eliminating unreachable symbols...");
        GrammarStats stats = new(grammar);

        EliminateNonGenerating(grammar);

        EliminateUnreachable(grammar);

        Logger.LogInfo("Unreachable symbols eliminated.");
        stats.LogDiff(grammar, Logger);
    }

    #region Nongenerating symbols/productions
    /// <summary>
    /// Eliminates all nongenerating symbols and productions in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to remove symbols from.</param>
    private void EliminateNonGenerating(Grammar grammar)
    {
        if (grammar.Productions.Count == 0)
        {
            return;
        }

        ISet<Nonterminal> nongeneratingSymbols = GetNongeneratingSymbols(grammar);
        
        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            Production production = grammar.Productions[i];
            if (nongeneratingSymbols.Contains(production.Head))
            {
                Logger.LogDebug($"Removing nongenerating production: {production}");
                grammar.RemoveProductionAt(i--);
            }
            else
            {
                foreach (ISymbol symbol in production.Body)
                {
                    if (nongeneratingSymbols.Contains(symbol))
                    {
                        Logger.LogDebug($"Removing nongenerating production: {production}");
                        grammar.RemoveProductionAt(i--);
                        break;
                    }
                }
            }
        }

        RemoveNongeneratingNonterminals(nongeneratingSymbols, grammar);

        if (grammar.Productions.Count == 0)
        {
            Logger.LogWarning("All productions were nongenerating.");
        }
    }

    /// <summary>
    /// Finds all nongenerating symbols in the grammar.
    /// </summary>
    /// <param name="grammar">The grammar to find nongenerating symbols in.</param>
    /// <returns>A set with all nongenerating symbols in the grammar.</returns>
    private ISet<Nonterminal> GetNongeneratingSymbols(Grammar grammar)
    {
        ISet<ISymbol> generatingSymbols = GetGeneratingSymbols(grammar);

        HashSet<Nonterminal> nongeneratingSymbols = new();
        foreach (Nonterminal nonterminal in grammar.Nonterminals)
        {
            if (!generatingSymbols.Contains(nonterminal))
            {
                Logger.LogDebug($"Found nongenerating symbol: {nonterminal}");
                nongeneratingSymbols.Add(nonterminal);
            }
        }

        return nongeneratingSymbols;
    }

    /// <summary>
    /// Finds all generating symbols in the given grammar.
    /// </summary>
    /// <param name="grammar">The grammar to find generating symbols in.</param>
    /// <returns>A set with all generating symbols in the grammar.</returns>
    private ISet<ISymbol> GetGeneratingSymbols(Grammar grammar)
    {
        HashSet<ISymbol> generatingSymbols = new();

        // All terminals are generating.
        foreach (Terminal terminal in grammar.Terminals)
        {
            Logger.LogDebug($"Found generating symbol: {terminal}");
            generatingSymbols.Add(terminal);
        }

        // Repeat the inductive step until no more generating symbols are found.
        int lastGeneratingSymbolCount;
        do
        {
            lastGeneratingSymbolCount = generatingSymbols.Count;
            foreach (Nonterminal nonterminal in grammar.Nonterminals)
            {
                if (generatingSymbols.Contains(nonterminal))
                {
                    continue;
                }

                if (IsNonterminalGenerating(generatingSymbols, grammar, nonterminal))
                {
                    Logger.LogDebug($"Found generating symbol: {nonterminal}");
                    generatingSymbols.Add(nonterminal);
                }
            }
        } while (lastGeneratingSymbolCount < generatingSymbols.Count);

        return generatingSymbols;
    }

    /// <summary>
    /// Checks whether a nonterminal is generating, meaning that it has at least one generating production.
    /// </summary>
    /// <param name="generatingSymbols">The known set of generating symbols.</param>
    /// <param name="grammar">The grammar to check in.</param>
    /// <param name="nonterminal">The nonterminal to check.</param>
    /// <returns><see langword="true"/> if the <paramref name="nonterminal"/> is generating, <see langword="false"/> otherwise.</returns>
    private static bool IsNonterminalGenerating(ISet<ISymbol> generatingSymbols, Grammar grammar, Nonterminal nonterminal)
    {
        foreach (Production production in grammar.GetProductionsByHead(nonterminal))
        {
            if (IsProductionGenerating(generatingSymbols, production))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks whether a production is generating, meaning that it either produces ε, or all of its symbols are generating.
    /// </summary>
    /// <param name="generatingSymbols">The set of all known generating symbols.</param>
    /// <param name="production">The production to check.</param>
    /// <returns><see langword="true"/> if the <paramref name="production"/> is generating, <see langword="false"/> otherwise.</returns>
    private static bool IsProductionGenerating(ISet<ISymbol> generatingSymbols, Production production)
    {
        if (production.Body.Count == 1 && production.Body[0] is EmptyString)
        {
            return true;
        }

        foreach (ISymbol symbol in production.Body)
        {
            if (!generatingSymbols.Contains(symbol))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Removes all nongenerating nonterminals from the grammar.
    /// </summary>
    /// <param name="nongeneratingSymbols">The set of nongenerating terminals.</param>
    /// <param name="grammar">The grammar to remove the nonterminals from.</param>
    private void RemoveNongeneratingNonterminals(ISet<Nonterminal> nongeneratingSymbols, Grammar grammar)
    {
        for (int i = 0; i < grammar.Nonterminals.Count; i++)
        {
            Nonterminal nonterminal = grammar.Nonterminals[i];
            if (nongeneratingSymbols.Contains(nonterminal))
            {
                Logger.LogDebug($"Removing nongenerating nonterminal: {nonterminal}");
                grammar.RemoveNonterminalAt(i--);
            }
        }
    }
    #endregion Nongenerating symbols/productions

    #region Unreachable symbols
    /// <summary>
    /// Eliminates all unreachable symbols in the grammar.
    /// </summary>
    /// <param name="grammar">The grammar to eliminate symbols from.</param>
    private void EliminateUnreachable(Grammar grammar)
    {
        if (grammar.Productions.Count == 0)
        {
            return;
        }

        ISet<ISymbol> reachableSymbols = GetReachableSymbols(grammar);

        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            if (!reachableSymbols.Contains(grammar.Productions[i].Head))
            {
                Logger.LogDebug($"Removing production {grammar.Productions[i]}: head is unreachable.");
                grammar.RemoveProductionAt(i--);
            }
        }

        for (int i = 0; i < grammar.Nonterminals.Count; i++)
        {
            Nonterminal nonterminal = grammar.Nonterminals[i];
            if (!reachableSymbols.Contains(nonterminal))
            {
                Logger.LogDebug($"Removing unreachable nonterminal {nonterminal}");
                grammar.RemoveNonterminalAt(i--);
            }
        }

        for (int i = 0; i < grammar.Terminals.Count; i++)
        {
            Terminal terminal = grammar.Terminals[i];
            if (!reachableSymbols.Contains(terminal))
            {
                Logger.LogDebug($"Removing unreachable terminal {terminal}");
                grammar.RemoveTerminalAt(i--);
            }
        }

        if (grammar.Productions.Count == 0)
        {
            Logger.LogWarning("All productions were unreachable.");
        }
    }

    /// <summary>
    /// Finds all reachable symbols in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to find reachable symbols in.</param>
    /// <returns>A set of all reachable symbols in the grammar.</returns>
    private ISet<ISymbol> GetReachableSymbols(Grammar grammar)
    {
        HashSet<ISymbol> reachableSymbols = new();

        // The start rule is assumed to be reachable.
        Logger.LogDebug($"Found reachable symbol: {grammar.Nonterminals[0]}");
        reachableSymbols.Add(grammar.Nonterminals[0]);

        // We inductively find further reachable symbols. This could have been a DFS or BFS, but we're sticking with the pattern in the book for now.
        int lastReachableCount;
        do
        {
            lastReachableCount = reachableSymbols.Count;

            foreach (Nonterminal nonterminal in grammar.Nonterminals)
            {
                if (!reachableSymbols.Contains(nonterminal))
                {
                    continue;
                }

                foreach (ISymbol symbol in grammar.GetProductionsByHead(nonterminal).SelectMany(p => p.Body))
                {
                    if (reachableSymbols.Add(symbol))
                    {
                        Logger.LogDebug($"Found reachable symbol: {symbol}");
                    }
                }
            }
        } while (lastReachableCount < reachableSymbols.Count);

        return reachableSymbols;
    }
    #endregion Unreachable symbols
}
