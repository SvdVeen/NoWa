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
        List<Expression> expressions = new(context._exprs.Count);
        foreach (var expr in context._exprs)
        {
            Expression newExpr = Grammar.CreateExpression();
            foreach (var sym in expr._symbols)
            {
                if (sym.t != null)
                    newExpr.AddTerminal(sym.t.value.Text);
                else if (sym.nt != null)
                    newExpr.AddNonterminal(sym.nt.value.Text);
                else
                    throw new InvalidOperationException("Parsed symbol is neither a terminal nor a nonterminal."); // This should not even be possible.
            }
            expressions.Add(newExpr);
        }

        _ = Grammar.CreateRule(context.name.value.Text, expressions.ToArray());
    }
}
