using NoWa.Common;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// 
/// <para>Currently only works for grammars without weighted attributes.</para>
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

        Console.WriteLine("Adding start rule...");
        AddStartRule(grammar);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();

        Console.WriteLine("Separating terminals...");
        SeparateTerminals(grammar);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();

        Console.WriteLine("Reducing nonterminals...");
        ReduceNonterminals(grammar);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();

        Console.WriteLine("Eliminating unit productions...");
        EliminateUnitProductions(grammar);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();

        Console.WriteLine("Conversion complete!");

        // TODO: Eliminating empty string productions, removing unreachable rules.
    }

    /// <summary>
    /// Replaces the start rule with a new one.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void AddStartRule(Grammar grammar)
    {
        Nonterminal nonterminal = grammar.GetRule(0).Nonterminal;
        Rule rule = grammar.InsertRule(0, "START");
        rule.Expressions.Add(new Expression(nonterminal));
    }

    /// <summary>
    /// Eliminates nonsolitary terminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void SeparateTerminals(Grammar grammar)
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
                        Nonterminal nonterminal = grammar.GetOrCreateNonterminal($"TERM-{terminal.Value.Replace(" ", "-")}");
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

    /// <summary>
    /// Splits rules with more than two nonterminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void ReduceNonterminals(Grammar grammar)
    {
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            foreach (var expr in rule.Expressions.Where(expr => expr.OfType<Nonterminal>().Count() > 2))
            {
                for (int j = expr.Count - 1;  j >= 2; j--)
                {
                    Nonterminal nonterminal = grammar.GetOrCreateNonterminal($"{expr[j - 1].Value}-{expr[j].Value}");
                    Rule newRule = new(nonterminal);
                    newRule.Expressions.Add(new(expr[j-1], expr[j]));
                    grammar.AddRule(newRule);
                    expr.RemoveAt(j);
                    expr.RemoveAt(j - 1);
                    expr.Add(nonterminal);
                }
            }
        }
    }

    /// <summary>
    /// Eliminates rules that only refer to a single nonterminal.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void EliminateUnitProductions(Grammar grammar)
    {
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            if (rule.Expressions.Count == 1 && rule.Expressions[0].Count == 1 && rule.Expressions[0][0] is Nonterminal nonterminal)
            {
                rule.Expressions.RemoveAt(0);
                foreach (var expr in grammar.GetRule(nonterminal.Value).Expressions)
                {
                    rule.Expressions.Add(new(expr.ToArray()));
                }
            }
        }
    }
}
