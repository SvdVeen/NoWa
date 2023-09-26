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
        int initialRuleCount = grammar.RuleCount;

        EliminateEmptyRules(grammar);
        // Start by finding nullables and trimming ε-productions.
        ISet<Nonterminal> nullables = GetNullables(grammar);
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            ProcessRule(nullables, grammar, i);
        }

        Logger.LogInfo("Eliminated ε-productions.");
        if (initialRuleCount != grammar.RuleCount)
        {
            Logger.LogInfo($"\tRemoved {initialRuleCount - grammar.RuleCount} rules");
        }
    }

    /// <summary>
    /// Removes rules that entirely consist of ε-productions iteratively until all rules that would only have ε-productions are eliminatied.
    /// </summary>
    /// <param name="grammar">The grammar to eliminate empty rules from.</param>
    private void EliminateEmptyRules(CFG grammar)
    {
        bool ruleRemoved;
        do
        {
            ruleRemoved = false;
            for (int i = 0; i < grammar.RuleCount; i++)
            {
                Rule rule = grammar.GetRule(i);
                for (int j = 0; j < rule.Productions.Count; j++)
                {
                    rule.Productions[j] = new(rule.Productions[j].Where(s => s is not EmptyString));
                    if (rule.Productions[j].Count == 0)
                    {
                        rule.Productions[j].Add(EmptyString.Instance);
                    }
                }
                MakeDistinct(rule.Productions);
                if (rule.Productions.Count == 1 && rule.Productions[0][0] is EmptyString)
                {
                    Logger.LogDebug($"Removing empty rule {rule.Nonterminal}");
                    ruleRemoved = true;
                    grammar.RemoveRuleAt(i--);
                    grammar.ReplaceSymbol(rule.Nonterminal, EmptyString.Instance, true);
                }
            }
        } while (ruleRemoved);
    }

    /// <summary>
    /// Filters a list of productions to only contain its distinct elements.
    /// </summary>
    /// <param name="productions">The list to filter.</param>
    private static void MakeDistinct(IList<Expression> productions)
    {
        List<Expression> distinct = productions.Distinct().ToList();
        if (distinct.Count < productions.Count)
        {
            productions.Clear();
            foreach (Expression production in distinct)
            {
                productions.Add(production);
            }
        }
    }

    #region obtaining nullables
    /// <summary>
    /// Gets the nonterminals in a grammar that have ε-productions in their rules. Reduces any occurrence of multiple instances of ε to a single instance of ε.
    /// </summary>
    /// <param name="grammar">The grammar to get the nullables for.</param>
    /// <returns>The set of all nullable nonterminals in the grammar.</returns>
    private ISet<Nonterminal> GetNullables(CFG grammar)
    {
        ISet<Nonterminal> nullables = GetFirstLevelNullables(grammar);
        int lastCount;
        do
        {
            lastCount = nullables.Count;
            for (int i = 0; i < grammar.RuleCount; i++)
            {
                Rule rule = grammar.GetRule(i);
                if (nullables.Contains(rule.Nonterminal))
                {
                    continue;
                }
                if (rule.Productions.All(e => e.OfType<Nonterminal>().Any(nullables.Contains)))
                {
                    Logger.LogDebug($"Found nullable: {rule}");
                    nullables.Add(rule.Nonterminal);
                }
            }
        } while (nullables.Count > lastCount);
        return nullables;
    }

    /// <summary>
    /// Gets all first-level nullables; those that directly produce ε. Removes duplicate instances of ε in productions.
    /// </summary>
    /// <param name="grammar">The grammar to get the nullables for.</param>
    /// <returns>A dictionary of all nullable nonterminals, with booleans indicating whether they produced only ε.</returns>
    private ISet<Nonterminal> GetFirstLevelNullables(CFG grammar)
    {
        HashSet<Nonterminal> nullables = new();
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            for (int j = 0; j < rule.Productions.Count; j++)
            {
                if (rule.Productions[j].Any((s) => s is EmptyString))
                {
                    Logger.LogDebug($"Production with ε: {rule.Productions[j]}");
                    rule.Productions[j] = new(rule.Productions[j].Where(s => s is not EmptyString));
                    if (rule.Productions[j].Count == 0)
                    {
                        rule.Productions[j].Add(EmptyString.Instance);
                    }
                    Logger.LogDebug($"Reduced to {rule.Productions[j]}");
                }
                if (rule.Productions[j].Count == 1 && rule.Productions[j][0] is EmptyString)
                {
                    if (nullables.Add(rule.Nonterminal))
                    {
                        Logger.LogDebug($"Found nullable: {rule.Nonterminal}");
                    }
                }
            }
        }
        return nullables;
    }
    #endregion obtaining nullables

    /// <summary>
    /// Removes all ε-productions from a rule by taking combinations of its productions where nullables are missing or not.
    /// </summary>
    /// <param name="nullables">All nullables in the grammar.</param>
    /// <param name="grammar">The grammar to convert.</param>
    /// <param name="ruleIndex">The index of the rule in the grammar to process.</param>
    private void ProcessRule(ISet<Nonterminal> nullables, CFG grammar, int ruleIndex)
    {
        Rule rule = grammar.GetRule(ruleIndex);
        HashSet<Expression> uniqueProductions = new();
        for (int i = 0; i < rule.Productions.Count; i++)
        {
            IList<Expression> combinations = ProcessProduction(nullables, uniqueProductions, rule.Productions[i]);
            rule.Productions.RemoveAt(i);
            for (int j = combinations.Count - 1; j >= 0; j--)
            {
                rule.Productions.Insert(i, combinations[j]);
            }
            i = (combinations.Count == 0) ? i - 1 : i;
        }
        if (rule.Productions.Count == 0)
        {
            Logger.LogDebug($"Removing empty rule {rule.Nonterminal}");
            grammar.RemoveRuleAt(ruleIndex);
        }
    }

    /// <summary>
    /// Shorthand for checking if any <see cref="ISymbol"/> is a nullable.
    /// </summary>
    /// <param name="nullables">The set of all nullable symbols.</param>
    /// <param name="symbol">The symbol to check.</param>
    /// <returns><see langword="true"/> if the <paramref name="symbol"/> is one of the <paramref name="nullables"/> or is the <see cref="EmptyString"/>; <see langword="false"/> otherwise.</returns>
    private static bool IsNullable(ISet<Nonterminal> nullables, ISymbol symbol) => symbol is EmptyString || (symbol is Nonterminal nt && nullables.Contains(nt));

    /// <summary>
    /// Removes all possible ε-productions by taking the combinations of a production where nullables are either absent or not.
    /// </summary>
    /// <param name="nullables">The nullables in the grammar.</param>
    /// <param name="uniqueProductions">The unique productions already present in the rule the <paramref name="production"/> is part of.</param>
    /// <param name="production">The production to process.</param>
    /// <returns>All combinations of the production with the <paramref name="nullables"/> missing or not.</returns>
    private List<Expression> ProcessProduction(ISet<Nonterminal> nullables, ISet<Expression> uniqueProductions, Expression production)
    {
        List<Expression> result = new();

        // Do not include direct empty string productions.
        if (production.Count == 1 && production[0] is EmptyString)
        {
            return result;
        }

        result.Add(production);
        for (int i = 0; i < production.Count; i++)
        {
            if (IsNullable(nullables, production[i]))
            {
                Expression newProduction = new(production);
                newProduction.RemoveAt(i);
                if (newProduction.Count > 0 && uniqueProductions.Add(newProduction))
                {
                    Logger.LogDebug($"Introducing new production: {newProduction}");
                    result.Add(newProduction);
                }
            }
        }
        return result;
    }
}
