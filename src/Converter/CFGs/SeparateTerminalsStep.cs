using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter.CFGs;

/// <summary>
/// A conversion step that separates all terminals into their own productions.
/// </summary>
public sealed class SeparateTerminalsStep : BaseConversionStep<CFG>
{
    /// <inheritdoc/>
    public SeparateTerminalsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Separates all terminals in productions of more than one symbol into their own productions.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(CFG grammar)
    {
        Logger.LogInfo("Separating terminals from large bodies...");
        GrammarStats stats = new(grammar);

        for (int i = 0; i < grammar.Productions.Count; i++)
        {
            SeparateTerminalsInProduction(grammar, grammar.Productions[i]);
        }

        Logger.LogInfo("Terminals separated.");
        stats.LogDiff(grammar, Logger);
    }

    /// <summary>
    /// Separate terminals in a production into their own rules.
    /// </summary>
    /// <param name="grammar">The grammar to separate the terminals in.</param>
    /// <param name="production">The production to separate the terminals in.</param>
    private void SeparateTerminalsInProduction(CFG grammar, Production production)
    {
        if (production.Body.Count <= 1)
        {
            return;
        }
        for (int i = 0; i < production.Body.Count; i++)
        {
            if (production.Body[i] is Terminal terminal)
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
    private void ReplaceTerminalInGrammar(CFG grammar, Terminal terminal)
    {
        Logger.LogDebug($"Replacing terminal: {terminal}");
        Production newProduction = new(Nonterminal.Get($"T-{terminal.Value}"), terminal);
        grammar.AddProduction(newProduction);
        Logger.LogDebug($"New nonterminal: {newProduction.Head}");

        foreach (Production production in grammar.Productions)
        {
            ReplaceTerminalInProduction(production, terminal, newProduction.Head);
        }
    }

    /// <summary>
    /// Replace all occurrences of a terminal in a production with a nonterminal.
    /// </summary>
    /// <param name="production">The production to replace the terminal in.</param>
    /// <param name="terminal">The terminal to replace.</param>
    /// <param name="nonterminal">The nonterminal to replace the terminal with.</param>
    private static void ReplaceTerminalInProduction(Production production, Terminal terminal, Nonterminal nonterminal)
    {
        if (production.Body.Count <= 1)
        {
            return;
        }
        for (int i = 0; i < production.Body.Count; ++i)
        {
            if (terminal.Equals(production.Body[i]))
            {
                production.Body[i] = nonterminal;
            }
        }
    }
}
