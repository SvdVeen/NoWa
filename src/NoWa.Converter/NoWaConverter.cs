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

        AddStartRule(grammar);

        SeparateTerminals(grammar);

        ReduceNonterminals(grammar);

        EliminateUnitProductions(grammar);

        EliminateEmptyStringProductions(grammar);

        // TODO: Removing unreachable rules.
        Console.WriteLine("Conversion complete!");
    }

    /// <summary>
    /// Replaces the start rule with a new one.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void AddStartRule(Grammar grammar)
    {
        Console.WriteLine("Adding start rule...");
        Nonterminal nonterminal = grammar.GetRule(0).Nonterminal;
        Rule rule = grammar.InsertRule(0, "START");
        rule.AddExpression(nonterminal);
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Eliminates nonsolitary terminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void SeparateTerminals(Grammar grammar)
    {
        Console.WriteLine("Separating terminals...");
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
                        newRule.AddExpression(terminal);
                        grammar.AddRule(newRule);
                    }
                }
            }
        }
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Splits rules with more than two nonterminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void ReduceNonterminals(Grammar grammar)
    {
        Console.WriteLine("Reducing nonterminals...");
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            foreach (var expr in rule.Expressions.Where(expr => expr.OfType<Nonterminal>().Count() > 2))
            {
                for (int j = expr.Count - 1;  j >= 2; j--)
                {
                    Nonterminal nonterminal = grammar.GetOrCreateNonterminal($"{expr[j - 1].Value}-{expr[j].Value}");
                    Rule newRule = new(nonterminal);
                    newRule.AddExpression(expr[j-1], expr[j]);
                    grammar.AddRule(newRule);
                    expr.RemoveAt(j);
                    expr.RemoveAt(j - 1);
                    expr.Add(nonterminal);
                }
            }
        }
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Eliminates rules that only refer to a single nonterminal.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void EliminateUnitProductions(Grammar grammar)
    {
        Console.WriteLine("Eliminating unit productions...");
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            if (rule.Expressions.Count == 1 && rule.Expressions[0].Count == 1 && rule.Expressions[0][0] is Nonterminal nonterminal)
            {
                rule.Expressions.RemoveAt(0);
                foreach (var expr in grammar.GetRule(nonterminal.Value).Expressions)
                {
                    rule.AddExpression(expr.ToArray());
                }
            }
        }
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Adjust rules so empty strings cannot be produced.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void EliminateEmptyStringProductions(Grammar grammar)
    {
        Console.WriteLine("Eliminating ε-productions...");
        // Start by finding nullables and trimming ε-productions.
        HashSet<Nonterminal> nullables = GetNullableNonterminals(grammar);
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            if (!nullables.Contains(rule.Nonterminal))
            {
                continue;
            }
            for (int j = 0, count = rule.Expressions.Count; j < count; j++)
            {
                Expression expr = rule.Expressions[j];
                for (int k = 0; k < expr.Count; k++)
                {
                    // I have to write an annoying thing to make all permutations of nullables happen. Agh
                }
            }
        }
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Gets all nullable nonterminals and trims empty string productions from them.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <returns>A set of all nullable nonterminals.</returns>
    private static HashSet<Nonterminal> GetNullableNonterminals(Grammar grammar)
    {
        HashSet<Nonterminal> nullables = new();
        // First, we find first-level nullables and trim their ε-productions.
        for (int i = 0; i < grammar.RuleCount; i++)
        {
            Rule rule = grammar.GetRule(i);
            for (int j = 0; j < rule.Expressions.Count; j++)
            {
                Expression expr = rule.Expressions[j];
                for (int k = 0; k < expr.Count; k++)
                {
                    if (expr[k] is EmptyString)
                    {
                        expr.RemoveAt(k--);
                    }
                }
                if (expr.Count == 0)
                {
                    nullables.Add(rule.Nonterminal);
                    rule.Expressions.RemoveAt(j--);
                }
            }
            if (rule.Expressions.Count == 0)
            {
                grammar.RemoveRule(i--);
            }
        }
        // We iteratively find all remaining nullable rules by seeing if all of their productions are nullable.
        int lastCount;
        do
        {
            lastCount = nullables.Count;
            for (int i = 0; i < grammar.RuleCount; i++)
            {
                Rule rule = grammar.GetRule(i);
                if (nullables.Contains(rule.Nonterminal))
                {
                    continue;
                }
                if (rule.Expressions.SelectMany(e => e.AsEnumerable()).All(s => s is Nonterminal n && nullables.Contains(n)))
                {
                    nullables.Add(rule.Nonterminal);
                }
            }
        } while (nullables.Count > lastCount);
        return nullables;
    }
}
