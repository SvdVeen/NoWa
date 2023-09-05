using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates all unit productions in a grammar.
/// </summary>
public sealed class UnitProductionsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public UnitProductionsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminates all unit productions in the given <see cref="Grammar"/>.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        var unitPairs = GetUnitPairs(grammar);

        // First, we save all original rules and replace them with versions that only contain their non-unit productions.
        List<Rule> originalRules = new(grammar.RuleCount);
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            originalRules.Add(rule);

            Rule newRule = new(rule.Nonterminal);
            foreach (Expression production in rule.Productions)
            {
                if (production.Count > 1 || production[0] is not Nonterminal)
                {
                    newRule.Productions.Add(production);
                }
            }

            grammar.RemoveRuleAt(i);
            if (i <= grammar.RuleCount - 1)
            {
                grammar.InsertRule(i, newRule);
            }
            else
            {
                grammar.AddRule(newRule);
            }
        }

        // Then, we go through the pairs and add all non-unit productions for unit pairs that aren't symmetrical.
        foreach (var pair in unitPairs)
        {
            Rule rule1 = grammar.GetRule(pair.Item1);
            Rule rule2 = originalRules.Where(r => r.Nonterminal.Equals(pair.Item2)).Single();
            
            if (!pair.Item1.Equals(pair.Item2))
            {
                for (int i = 0; i < rule2.Productions.Count; i++)
                {
                    if (rule2.Productions[i].Count > 1 || rule2.Productions[i][0] is not Nonterminal)
                    {
                        rule1.Productions.Add(rule2.Productions[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets all unit pairs in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to get unit pairs from.</param>
    /// <returns>A set of all unit pairs in the grammar.</returns>
    private ISet<Tuple<Nonterminal,Nonterminal>> GetUnitPairs(Grammar grammar)
    {
        HashSet<Tuple<Nonterminal,Nonterminal>> pairs = new();
        // Base step: every nonterminal pairs with itself.
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            if (pairs.Add(new(rule.Nonterminal, rule.Nonterminal)))
            {
                Logger.LogDebug($"Adding unit pair ({rule.Nonterminal}, {rule.Nonterminal})");
            }
        }

        // Induction step: if there exists a unit pair (A,B), and a production B -> C, where C is a variable, then (A,C) is a unit pair.
        // Repeat this until no more new pairs are found.
        HashSet<Tuple<Nonterminal, Nonterminal>> oldPairs;
        do
        {
            oldPairs = new(pairs);
            foreach (var pair in pairs.ToArray())
            {
                Rule rule = grammar.GetRule(pair.Item2);
                for (int i = 0; i < rule.Productions.Count; i++)
                {
                    if (rule.Productions[i].Count == 1 && rule.Productions[i][0] is Nonterminal nonterminal 
                        && pairs.Add(new(pair.Item1, nonterminal)))
                    {
                        Logger.LogDebug($"Adding unit pair ({pair.Item1}, {nonterminal})");
                    }
                }
            }
        } while (oldPairs.Count < pairs.Count);

        return pairs;
    }
}
