﻿using Antlr4.Runtime.Misc;
using NoWa.Common;

namespace NoWa.Parser;

/// <summary>
/// Used to construct a <see cref="Common.CFG"/> from a parsed grammar.
/// </summary>
internal class NoWaListener : Generated.NoWaParserBaseListener
{
    /// <summary>
    /// The grammar generated by the listener.
    /// </summary>
    internal CFG Grammar { get; } = new();

    /// <summary>
    /// Exit a parse tree produced by <see cref="Generated.NoWaParser.RuleContext"/>.
    /// 
    /// <para>Creates a new <see cref="Rule"/> and adds it to the grammar.</para>
    /// </summary>
    /// <param name="context"></param>
    public override void ExitRule([NotNull] Generated.NoWaParser.RuleContext context)
    {
        Rule rule = Grammar.AddRule(context.nonterminal.Text);
        foreach (var expr in context._exprs)
        {
            Expression newExpr = new();
            foreach (var sym in expr._symbols)
            {
                if (sym is Generated.NoWaParser.NonterminalContext nonterminal)
                {
                    newExpr.Add(Grammar.AddNonterminal(nonterminal.value.Text));
                }
                else if (sym is Generated.NoWaParser.TerminalContext terminal)
                {
                    newExpr.Add(Grammar.AddTerminal(terminal.value.Text));
                }
                else if (sym is Generated.NoWaParser.EmptyStringContext)
                {
                    newExpr.Add(EmptyString.Instance);
                }
                else
                {
                    throw new InvalidOperationException("Parsed symbol is invalid."); // This should not even be possible.
                }
            }
            rule.Productions.Add(newExpr);
        }
    }
}
