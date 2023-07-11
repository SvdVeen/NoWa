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
    public IList<Expression> Expressions { get; } = new List<Expression>();

    /// <summary>
    /// Creates a new instance of the <see cref="Rule"/> class.
    /// </summary>
    /// <param name="nonterminal">The <see cref="Common.Nonterminal"/> associated with the new rule.</param>
    public Rule(Nonterminal nonterminal) => Nonterminal = nonterminal;

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Expressions.Count == 0 || Expressions[0].Count == 0)
        {
            return $"Empty rule {Nonterminal}";
        }

        return new StringBuilder($"{Nonterminal} = ").AppendJoin(" | ", Expressions).Append(" ;").ToString();
    }
}
