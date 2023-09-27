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
    public override void Convert(CFG grammar)
    {
        Logger.LogInfo("Splitting production bodies longer than 2...");
        int initialRuleCount = grammar.RuleCount;
        int initialNonterminalCount = grammar.Nonterminals.Count;

        Dictionary<string, Nonterminal> newNonterminals = new();
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            SplitRule(grammar, newNonterminals, grammar.GetRule(i));
        }

        Logger.LogInfo("Production bodies longer than 2 split.");
        if (initialRuleCount != grammar.RuleCount)
        {
            Logger.LogInfo($"\tIntroduced {grammar.RuleCount - initialRuleCount} rules.");
        }
        if (initialNonterminalCount != grammar.Nonterminals.Count)
        {
            Logger.LogInfo($"\tIntroduced {grammar.Nonterminals.Count - initialNonterminalCount} nonterminals.");
        }
    }

    /// <summary>
    /// Splits all productions in a rule if they are longer than 3 symbols.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <param name="newNonterminals">The newly introduced nonterminals in the step.</param>
    /// <param name="rule">The rule to split productions in.</param>
    private void SplitRule(CFG grammar, IDictionary<string, Nonterminal> newNonterminals, Rule rule)
    {
        foreach (Expression production in rule.Productions)
        {
            SplitProduction(grammar, newNonterminals, production);
        }
    }

    /// <summary>
    /// Splits a production into smaller ones if it is longer than 3 symbols.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <param name="newNonterminals">The newly introduced nonterminals in the step.</param>
    /// <param name="production">The production to split.</param>
    private void SplitProduction(CFG grammar, IDictionary<string, Nonterminal> newNonterminals, Expression production)
    {
        while (production.Count > 2)
        {
            Logger.LogDebug($"Replacing in production {production}.");
            string newName = $"{production[^2]}-{production[^1]}";
            if (!newNonterminals.TryGetValue(newName, out Nonterminal? newNonterminal))
            {
                Rule newRule = grammar.AddRule(newName);
                newRule.AddProduction(production[^2], production[^1]);
                newNonterminal = newRule.Nonterminal;
                newNonterminals.Add(newName, newNonterminal);
                Logger.LogDebug($"Added rule {newRule}");
            }
            production.RemoveAt(production.Count - 1);
            production.RemoveAt(production.Count - 1);
            production.Add(newNonterminal);
            Logger.LogDebug($"New production: {production}");
        }
    }
}
