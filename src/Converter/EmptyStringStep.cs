using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates all ε-productions in a grammar.
/// </summary>
public sealed class EmptyStringStep : BaseConversionStep, IConversionStep
{
    /// <inheritdoc/>
    public EmptyStringStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminate all empty string productions in the given <see cref="Grammar"/>.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        Logger.LogInfo("Eliminating ε-productions...");
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
                grammar.RemoveRuleAt(i--);
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
