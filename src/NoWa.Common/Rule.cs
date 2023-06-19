using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a rule in a grammar.
/// </summary>
public class Rule
{
    /// <summary>
    /// Gets or sets the nonterminal this rule is associated with.
    /// </summary>
    public Nonterminal Nonterminal { get; set; }

    /// <summary>
    /// Gets the list of expressions this nonterminal translates to.
    /// </summary>
    public IList<Expression> Expressions { get; }

    /// <summary>
    /// Creates a new instance of a rule with the given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal the rule is associated with.</param>
    /// <param name="expressions">The expressions the nonterminal translates to.</param>
    public Rule(Nonterminal nonterminal, IEnumerable<Expression> expressions)
    {
        Nonterminal = nonterminal;
        Expressions = new List<Expression>(expressions);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">The rule contains no expressions.</exception>
    public override string ToString()
    {
        if (Expressions.Count == 0)
            throw new InvalidOperationException($"Rule {Nonterminal} contains no expressions.");

        StringBuilder sb = new StringBuilder();
        _ = sb.Append($"{Nonterminal} ::= {Expressions[0]}");
        for (int i = 1; i < Expressions.Count; i++)
            _ = sb.Append($" | {Expressions[i]}");
        return sb.Append(" ;").ToString();
    }
}
