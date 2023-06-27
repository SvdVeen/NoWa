using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a rule in a grammar.
/// </summary>
public class Rule
{
    private readonly Grammar _grammar;
    private readonly int _nonterminal;

    /// <summary>
    /// Gets or sets the nonterminal this rule is associated with.
    /// </summary>
    public Nonterminal Nonterminal 
    { 
        get 
        {
            if (_grammar.GetSymbol(_nonterminal) is not Nonterminal nonterminal)
                throw new InvalidOperationException($"Rule nonterminal is not a nonterminal.");
            return nonterminal;
        } 
    }

    /// <summary>
    /// Gets the list of expressions this nonterminal translates to.
    /// </summary>
    public IList<Expression> Expressions { get; }

    /// <summary>
    /// Creates a new instance of a rule.
    /// </summary>
    /// <param name="grammar">The grammar this rule belongs to.</param>
    /// <param name="nonterminal">The index of the nonterminal this rule is associated with.</param>
    /// <param name="exprs">The expressions the nonterminal translates to.</param>
    internal Rule(Grammar grammar, int nonterminal, IList<Expression> exprs)
    {
        _grammar = grammar;
        _nonterminal = nonterminal;
        Expressions = exprs;
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">The rule contains no expressions.</exception>
    public override string ToString()
    {
        if (Expressions.Count == 0)
            throw new InvalidOperationException($"Rule {Nonterminal} contains no expressions.");
        if (Expressions.Count == 0)
            throw new InvalidOperationException($"First expression in rule {Nonterminal} contains no symbols.");

        StringBuilder sb = new();
        _ = sb.Append($"{Nonterminal} ::= {Expressions[0]}");
        for (int i = 1; i < Expressions.Count; i++)
            _ = sb.Append($" | {Expressions[i]}");
        return sb.Append(" ;").ToString();
    }
}
