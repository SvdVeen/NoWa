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
    public override void Convert(CFG grammar)
    {
        Logger.LogInfo("Eliminating unreachable symbols...");
        int initialRuleCount = grammar.RuleCount;
        int initialNonterminalCount = grammar.NonterminalCount;
        int initialTerminalCount = grammar.TerminalCount;

        EliminateNonGenerating(grammar);
        EliminateUnreachable(grammar);

        Logger.LogInfo("Unreachable symbols eliminated.");
        if (initialRuleCount != grammar.RuleCount)
        {
            Logger.LogInfo($"\tRemoved {initialRuleCount - grammar.RuleCount} rules");
        }
        if (initialNonterminalCount != grammar.NonterminalCount)
        {
            Logger.LogInfo($"\tRemoved {initialNonterminalCount - grammar.NonterminalCount} nonterminals");
        }
        if (initialTerminalCount != grammar.TerminalCount)
        {
            Logger.LogInfo($"\tRemoved {initialTerminalCount - grammar.TerminalCount} terminals");
        }
    }

    #region Nongenerating symbols/productions

    /// <summary>
    /// Eliminates all nongenerating symbols and productions in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to remove symbols from.</param>
    private void EliminateNonGenerating(CFG grammar)
    {
        if (grammar.RuleCount == 0)
        {
            return;
        }

        ISet<Nonterminal> nongeneratingSymbols = GetNongeneratingSymbols(grammar);

        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            if (nongeneratingSymbols.Contains(rule.Nonterminal))
            {
                Logger.LogDebug($"Removing rule and nonterminal {rule.Nonterminal}: nonterminal is nongenerating");
                grammar.RemoveRuleAt(i--);
            }
            else
            {
                for (int j = 0; j < rule.Productions.Count; j++)
                {
                    foreach (ISymbol symbol in rule.Productions[j])
                    {
                        if (symbol is Nonterminal nonterminal && nongeneratingSymbols.Contains(nonterminal))
                        {
                            Logger.LogDebug($"Removing production {rule.Nonterminal} = {rule.Productions[j]}: contained nongenerating nonterminal {nonterminal}");
                            rule.Productions.RemoveAt(j--);
                            break;
                        }
                    }
                }
            }
        }

        RemoveNongeneratingNonterminals(nongeneratingSymbols, grammar);

        if (grammar.RuleCount == 0)
        {
            Logger.LogWarning("Eliminating nongenerating symbols removed all productions.");
        }
    }

    /// <summary>
    /// Finds all nongenerating symbols in the grammar.
    /// </summary>
    /// <param name="grammar">The grammar to find nongenerating symbols in.</param>
    /// <returns>A set with all nongenerating symbols in the grammar.</returns>
    private ISet<Nonterminal> GetNongeneratingSymbols(CFG grammar)
    {
        ISet<ISymbol> generatingSymbols = GetGeneratingSymbols(grammar);
        HashSet<Nonterminal> nongeneratingSymbols = new();
        for (int i = 0; i < grammar.NonterminalCount; i++)
        {
            Nonterminal nonterminal = grammar.GetNonterminal(i);
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
    private ISet<ISymbol> GetGeneratingSymbols(CFG grammar)
    {
        HashSet<ISymbol> generatingSymbols = new();

        // All terminals are generating.
        for (int i = 0; i < grammar.TerminalCount; i++)
        {
            Terminal terminal = grammar.GetTerminal(i);
            Logger.LogDebug($"Found generating symbol: {terminal}");
            generatingSymbols.Add(terminal);
        }

        // Repeat the inductive step until no more generating symbols are found.
        int lastGeneratingSymbolCount;
        do
        {
            lastGeneratingSymbolCount = generatingSymbols.Count;

            for (int i = 0; i < grammar.RuleCount; i++)
            {
                Rule rule = grammar.GetRule(i);
                if (generatingSymbols.Contains(rule.Nonterminal))
                {
                    continue;
                }

                if (IsRuleGenerating(generatingSymbols, rule))
                {
                    Logger.LogDebug($"Found generating symbol: {rule.Nonterminal}");
                    generatingSymbols.Add(rule.Nonterminal);
                }
            }
        } while (lastGeneratingSymbolCount < generatingSymbols.Count);

        return generatingSymbols;
    }

    /// <summary>
    /// Checks whether a rule is generating, meaning that any of its productions are generating.
    /// </summary>
    /// <param name="generatingSymbols">The set of all generating symbols.</param>
    /// <param name="rule">The rule to check.</param>
    /// <returns><see langword="true"/> if any of the productions in the rule are generating; <see langword="false"/> otherwise.</returns>
    private static bool IsRuleGenerating(ISet<ISymbol> generatingSymbols, Rule rule)
    {
        foreach (Expression production in rule.Productions)
        {
            if (IsProductionGenerating(generatingSymbols, production))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks whether a production is generating, meaning that all of its symbols are generating symbols.
    /// </summary>
    /// <param name="generatingSymbols">The set of generating symbols.</param>
    /// <param name="production">The production to check.</param>
    /// <returns><see langword="true"/> if all symbols in the production are generating; <see langword="false"/> otherwise.</returns>
    private static bool IsProductionGenerating(ISet<ISymbol> generatingSymbols, Expression production)
    {
        bool allGenerating = true;
        foreach (ISymbol symbol in production)
        {
            if (!generatingSymbols.Contains(symbol))
            {
                allGenerating = false;
                break;
            }
        }
        return allGenerating;
    }

    /// <summary>
    /// Removes all nongenerating nonterminals from the grammar.
    /// </summary>
    /// <param name="nongeneratingSymbols">The set of nongenerating terminals.</param>
    /// <param name="grammar">The grammar to remove the nonterminals from.</param>
    private void RemoveNongeneratingNonterminals(ISet<Nonterminal> nongeneratingSymbols, CFG grammar)
    {
        for (int i = 0; i < grammar.NonterminalCount; i++)
        {
            Nonterminal nonterminal = grammar.GetNonterminal(i);
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
    private void EliminateUnreachable(CFG grammar)
    {
        if (grammar.RuleCount == 0)
        {
            return;
        }

        ISet<ISymbol> reachableSymbols = GetReachableSymbols(grammar);

        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            if (!reachableSymbols.Contains(rule.Nonterminal))
            {
                Logger.LogDebug($"Removing rule and nonterminal {rule.Nonterminal}: rule is unreachable.");
                grammar.RemoveRuleAt(i--);
            }
        }

        for (int i = 0; i < grammar.NonterminalCount; i++)
        {
            Nonterminal nonterminal = grammar.GetNonterminal(i);
            if (!reachableSymbols.Contains(nonterminal))
            {
                Logger.LogDebug($"Removing unreachable nonterminal {nonterminal}");
                grammar.RemoveNonterminalAt(i--);
            }
        }

        for (int i = 0; i < grammar.TerminalCount; i++)
        {
            Terminal terminal = grammar.GetTerminal(i);
            if (!reachableSymbols.Contains(terminal))
            {
                Logger.LogDebug($"Removing unreachable terminal {terminal}");
                grammar.RemoveTerminalAt(i--);
            }
        }

        if (grammar.RuleCount == 0)
        {
            Logger.LogWarning("Eliminating unreachable symbols removed all productions.");
        }
    }

    /// <summary>
    /// Finds all reachable symbols in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to find reachable symbols in.</param>
    /// <returns>A set of all reachable symbols in the grammar.</returns>
    private ISet<ISymbol> GetReachableSymbols(CFG grammar)
    {
        HashSet<ISymbol> reachableSymbols = new();

        // The start rule is assumed to be reachable.
        Logger.LogDebug($"Found reachable symbol: {grammar.GetRule(0).Nonterminal}");
        reachableSymbols.Add(grammar.GetRule(0).Nonterminal);

        // We inductively find further reachable symbols. This could have been a DFS or BFS, but we're sticking with the pattern in the book for now.
        int lastReachableCount;
        do
        {
            lastReachableCount = reachableSymbols.Count;

            for (int i = 0; i < grammar.RuleCount; i++)
            {
                Rule rule = grammar.GetRule(i);
                if (!reachableSymbols.Contains(rule.Nonterminal))
                {
                    continue;
                }

                foreach (ISymbol symbol in rule.Productions.SelectMany(p => p))
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
