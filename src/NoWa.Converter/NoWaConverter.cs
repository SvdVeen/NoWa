using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// 
/// <para>Currently only works for grammars without weighted attributes.</para>
/// </summary>
public class NoWaConverter
{
    /// <summary>
    /// The steps used in the conversion.
    /// </summary>
    private readonly IList<IConversionStep> _steps;

    /// <summary>
    /// The logger used by the converter.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Create a new instance of the converter with the given <see cref="ILogger"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging </param>
    public NoWaConverter(ILogger logger)
    {
        Logger = logger;
        _steps = new List<IConversionStep>() { };
    }

    /// <summary>
    /// Convert the given <see cref="Grammar"/> to Chomsky Normal Form.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <returns>The converted grammar.</returns>
    public Grammar? Convert(Grammar grammar, bool continueOnError = false)
    {
        Grammar result = grammar;
        Logger.LogInfo("Starting CNF conversion...");
        Logger.LogDebug($"Initial grammar:{Environment.NewLine}{result}");

        for (int i = 0; i < _steps.Count; i++)
        {
            Logger.LogInfo($"Step {i + 1} of {_steps.Count}");
            try
            {
                _steps[i].Convert(grammar);
                Logger.LogDebug($"Intermediate grammar:{Environment.NewLine}{result}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Conversion encountered an unexpected error:{Environment.NewLine}\t{ex.GetType().Name} - {ex.Message}");
                if (!continueOnError)
                {
                    return null;
                }
            }
        }

        return result;
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
            HashSet<Expression> uniqueExpressions = new();
            for (int j = 0; j < rule.Expressions.Count; j++)
            {
                Expression expr = rule.Expressions[j];
                List<Nonterminal> exprNullables = new();
                for (int k = 0; k < expr.Count; k++)
                {
                    if (nullables.Contains(expr[k]) && !exprNullables.Contains(expr[k]))
                    {
                        exprNullables.Insert(0, (Nonterminal)expr[k]);
                    }
                }
                if (exprNullables.Count > 0)
                {
                    IList<Expression> newExprs = GetExpressionPermutations(uniqueExpressions, expr, exprNullables);
                    rule.Expressions.RemoveAt(j);
                    for (int k = newExprs.Count - 1; k >= 0; k--)
                    {
                        rule.Expressions.Insert(j, newExprs[k]);
                    }
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
                if (rule.Expressions.All(e => e.Any(s => s is Nonterminal n && nullables.Contains(n))))
                {
                    nullables.Add(rule.Nonterminal);
                }
            }
        } while (nullables.Count > lastCount);
        return nullables;
    }

    /// <summary>
    /// Get all permutation of an expression with certain nullable nonterminals either existing or not existing.
    /// </summary>
    /// <param name="uniqueExpressions">A set of current unique expressions, used to prevent duplicates from being inserted.</param>
    /// <param name="expression">The expression to get permutations of.</param>
    /// <param name="nullables">The nullable nonterminals to process.</param>
    /// <returns>A collection of all versions of the expression with the given nonterminals either existing or not existing.</returns>
    private static IList<Expression> GetExpressionPermutations(HashSet<Expression> uniqueExpressions, Expression expression, IEnumerable<Nonterminal> nullables)
    {
        List<Expression> exprs = new() { expression };
        foreach (var nullable in nullables)
        {
            for (int i = 0; i < exprs.Count; i++)
            {
                exprs.AddRange(GetNullablePermutations(uniqueExpressions, exprs[i], nullable));
            }
        }
        return exprs;
    }

    /// <summary>
    /// Gets all permutations of a nullable nonterminal existing or not existing.
    /// </summary>
    /// <param name="uniqueExprs">A hashset of current unique expressions, used to prevent duplicates.</param>
    /// <param name="expr">The expression to get the permutations of.</param>
    /// <param name="nullable">The nullable nonterminal to process.</param>
    /// <returns>A collection of all expressions with the nonterminal either existing or not existing.</returns>
    private static IEnumerable<Expression> GetNullablePermutations(HashSet<Expression> uniqueExprs, Expression expr, Nonterminal nullable)
    {
        List<Expression> exprs = new();
        for (int i = 0; i < expr.Count; i++)
        {
            if (expr[i].Equals(nullable))
            {
                Expression newExpr = new(expr.ToArray());
                newExpr.RemoveAt(i);
                if (newExpr.Count > 0 && uniqueExprs.Add(newExpr))
                {
                    exprs.Add(newExpr);
                }
            }
        }
        return exprs;
    }
}
