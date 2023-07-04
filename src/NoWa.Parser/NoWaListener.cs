using Antlr4.Runtime.Misc;
using NoWa.Common;
using NoWa.Parser.Generated;

namespace NoWa.Parser;

/// <summary>
/// Used to construct a <see cref="Common.Grammar"/> from a parsed grammar.
/// </summary>
internal class NoWaListener : NoWaParserBaseListener
{
    internal Grammar Grammar { get; } = new();

    /// <summary>
    /// Exit a parse tree produced by <see cref="Generated.NoWaParser.RuleContext"/>.
    /// 
    /// <para>Creates a new <see cref="Rule"/> and adds it to the grammar.</para>
    /// </summary>
    /// <param name="context"></param>
    public override void ExitRule([NotNull] Generated.NoWaParser.RuleContext context)
    {
        Rule rule = Grammar.AddRule(context.name.value.Text);
        foreach (var expr in context._exprs)
        {
            Expression newExpr = new();
            foreach (var sym in expr._symbols)
            {
                if (sym.t != null)
                {
                    newExpr.Add(Grammar.GetOrCreateTerminal(sym.t.value.Text));
                }
                else if (sym.nt != null)
                {
                    newExpr.Add(Grammar.GetOrCreateNonterminal(sym.nt.value.Text));
                }
                else
                {
                    throw new InvalidOperationException("Parsed symbol is neither a terminal nor a nonterminal."); // This should not even be possible.
                }
            }
            rule.Expressions.Add(newExpr);
        }
    }
}
