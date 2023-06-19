using Antlr4.Runtime.Misc;
using NoWa.Common;
using NoWa.Parser.Generated;

namespace NoWa.Parser;

/// <summary>
/// Used to construct a <see cref="NoWa.Common.Grammar"/> for a 
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
        Nonterminal nonterminal = Grammar.GetNonterminal(context.name.value.Text);

        List<Expression> expressions = new List<Expression>(context._exprs.Count);
        foreach (var expr in context._exprs)
        {
            Expression newExpr = new Expression();
            foreach (var sym in expr._symbols)
            {
                if (sym.t != null)
                    newExpr.Symbols.Add(new Terminal(sym.t.value.Text));
                else if (sym.nt != null)
                    newExpr.Symbols.Add(new Nonterminal(sym.nt.value.Text));
                else
                    throw new InvalidOperationException("Parsed symbol is neither a terminal nor a nonterminal."); // This should not even be possible.
            }
            expressions.Add(newExpr);
        }

        Rule rule = new Rule(nonterminal, expressions);
        Grammar.AddRule(nonterminal, rule);
    }

    /// <summary>
    /// Enter a parse tree produced by <see cref="Generated.NoWaParser.NonterminalContext"/>.
    /// 
    /// <para>Creates a new <see cref="Nonterminal"/> and adds it to the grammar.</para>
    /// </summary>
    /// <inheritdoc/>
    public override void EnterNonterminal([NotNull] Generated.NoWaParser.NonterminalContext context)
    {
        Grammar.AddNonterminal(new Nonterminal(context.value.Text));
    }
}
