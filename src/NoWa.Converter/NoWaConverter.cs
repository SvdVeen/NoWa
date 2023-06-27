using NoWa.Common;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// </summary>
public class NoWaConverter
{
    /// <summary>
    /// Convert a given grammar to CNF.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void Convert(Grammar grammar)
    {
        Console.WriteLine(grammar.ToString());
        Start(grammar);
        Console.WriteLine(grammar.ToString());
        Term(grammar);
        Console.WriteLine(grammar.ToString());
    }

    /// <summary>
    /// Replaces the start rule with a new one.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void Start(Grammar grammar)
    {
        Rule rule = grammar.CreateRule("NoWa-START", grammar.CreateExpression(grammar.GetRule(0).Nonterminal));
        //grammar.InsertRule(0, rule);
    }

    /// <summary>
    /// Eliminates nonsolitary terminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void Term(Grammar grammar)
    {
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            for (int j = 0; j < rule.Expressions.Count; j++)
            {
                Expression expr = rule.Expressions[j];
                if (expr.Count > 1)
                {
                    foreach (ISymbol sym in expr.Symbols)
                    {
                        if (sym is Terminal terminal)
                        {
                            Nonterminal nonterminal = grammar.GetOrCreateNonterminal($"NoWa-TERM-{terminal.Value.Replace(" ", "-")}");
                            grammar.ReplaceSymbol(terminal, nonterminal);
                            _ = grammar.CreateRule(nonterminal.Value, grammar.CreateExpression(terminal));
                        }
                    }
                }
            }
        }
    }
}
