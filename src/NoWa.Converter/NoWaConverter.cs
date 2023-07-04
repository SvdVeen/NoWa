using NoWa.Common;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// </summary>
public static class NoWaConverter
{
    /// <summary>
    /// Convert a given grammar to CNF.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void Convert(Grammar grammar)
    {
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();

        Start(grammar);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();

        Term(grammar);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Replaces the start rule with a new one.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void Start(Grammar grammar)
    {
        Nonterminal nonterminal = grammar.GetRule(0).Nonterminal;
        Rule rule = grammar.InsertRule(0, "NoWa-START");
        rule.Expressions.Add(new Expression(nonterminal));
    }

    /// <summary>
    /// Eliminates nonsolitary terminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void Term(Grammar grammar)
    {
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            foreach (var expr in rule.Expressions.Where(expr => expr.Count > 1))
            {
                for (int j = 0; j < expr.Count; j++)
                {
                    if (expr[j] is Terminal terminal)
                    {
                        // A nonsolitary terminal is replaced with a nonterminal across the entire grammar.
                        Nonterminal nonterminal = grammar.GetOrCreateNonterminal($"NoWa-TERM-{terminal.Value.Replace(" ", "-")}");
                        grammar.ReplaceSymbol(terminal, nonterminal, false); // Keep the original because we will insert it again in the new rule.
                        // A new rule is added for the new nonterminal that refers to the old terminal.
                        Rule newRule = new(nonterminal);
                        newRule.Expressions.Add(new(terminal));
                        grammar.AddRule(newRule);
                    }
                }
            }
        }
    }
}
