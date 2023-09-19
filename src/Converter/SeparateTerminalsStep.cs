using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that separates all terminals into their own productions.
/// </summary>
public sealed class SeparateTerminalsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public SeparateTerminalsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Separates all terminals in productions of more than one symbol into their own productions.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        Logger.LogInfo("Separating terminals from large bodies...");
        int initialRuleCount = grammar.RuleCount;

        for (int i = 0; i < grammar.RuleCount; i++)
        {
            SeparateTerminalsInRule(grammar, grammar.GetRule(i));
        }

        Logger.LogInfo("Terminals separated.");
        if (initialRuleCount !=  grammar.RuleCount)
        {
            Logger.LogInfo($"\tIntroduced {grammar.RuleCount - initialRuleCount} rules.");
        }
    }

    /// <summary>
    /// Separate terminals in a rule into their own rules.
    /// </summary>
    /// <param name="grammar">The grammar to separate terminals in.</param>
    /// <param name="rule">The rule to separate terminals in.</param>
    private void SeparateTerminalsInRule(Grammar grammar, Rule rule)
    {
        foreach (Expression production in rule.Productions)
        {
            if (production.Count == 1)
            {
                continue;
            }
            SeparateTerminalsInProduction(grammar, production);
        }
    }

    /// <summary>
    /// Separate terminals in a production into their own rules.
    /// </summary>
    /// <param name="grammar">The grammar to separate the terminals in.</param>
    /// <param name="production">The production to separate the terminals in.</param>
    private void SeparateTerminalsInProduction(Grammar grammar, Expression production)
    {
        for (int i = 0; i < production.Count; i++)
        {
            if (production[i] is Terminal terminal)
            {
                ReplaceTerminalInGrammar(grammar, terminal);
            }
        }
    }
    /// <summary>
    /// Replace a terminal in bodies larger than 2 with a nonterminal that produces it.
    /// </summary>
    /// <param name="grammar">The grammar to replace the terminal in.</param>
    /// <param name="terminal">The terminal to replace.</param>
    private void ReplaceTerminalInGrammar(Grammar grammar, Terminal terminal)
    {
        Logger.LogDebug($"Replacing terminal: {terminal}");
        Rule newRule = grammar.AddRule($"T-{terminal.Value}"); // This could fail if a rule with that name already existed, but that is pretty unlikely. If it does: too bad!
        newRule.AddProduction(terminal);
        Logger.LogDebug($"New nonterminal: {newRule.Nonterminal}");

        for (int i = 0; i < grammar.RuleCount; ++i)
        {
            ReplaceTerminalInRule(grammar.GetRule(i), terminal, newRule.Nonterminal);
        }
    }

    /// <summary>
    /// Replace all occurrences of a terminal in a rule with a nonterminal.
    /// </summary>
    /// <param name="rule">The rule to replace the terminal in.</param>
    /// <param name="terminal">The terminal to replace.</param>
    /// <param name="nonterminal">The nonterminal to replace the terminal with.</param>
    private void ReplaceTerminalInRule(Rule rule, Terminal terminal, Nonterminal nonterminal)
    {
        foreach (Expression production in rule.Productions)
        {
            if (production.Count <= 1)
            {
                continue;
            }
            ReplaceTerminalInProduction(production, terminal, nonterminal);
        }
    }

    /// <summary>
    /// Replace all occurrences of a terminal in a production with a nonterminal.
    /// </summary>
    /// <param name="production">The production to replace the terminal in.</param>
    /// <param name="terminal">The terminal to replace.</param>
    /// <param name="nonterminal">The nonterminal to replace the terminal with.</param>
    private void ReplaceTerminalInProduction(Expression production, Terminal terminal, Nonterminal nonterminal)
    {
        for (int i = 0; i < production.Count; ++i)
        {
            if (terminal.Equals(production[i]))
            {
                production[i] = nonterminal;
            }
        }
    }
}
