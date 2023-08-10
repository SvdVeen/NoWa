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

    /// <summary>
    /// Add an expression to the rule.
    /// </summary>
    /// <param name="symbols">The symbols in the expression.</param>
    /// <remarks>Used as notational shorthand to make quick instantiations more readable.</remarks>
    public void AddExpression(params ISymbol[] symbols) => Expressions.Add(new(symbols));


    /// <summary>
    /// Replace all occurrences of a symbol in this rule's expressions or its nonterminal with another.
    /// </summary>
    /// <param name="symbol">The symbol to replace.</param>
    /// <param name="newSymbol">The symbol to replace the original with.</param>
    public void ReplaceSymbol(ISymbol symbol, ISymbol newSymbol)
    {
        if (newSymbol is Nonterminal nt && Nonterminal.Value == symbol.Value)
        {
            Nonterminal = nt;
        }

        foreach (Expression expression in Expressions)
        {
            expression.Replace(symbol, newSymbol);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Expressions.Count == 0)
        {
            return $"Empty rule {Nonterminal}";
        }

        return new StringBuilder($"{Nonterminal} = ").AppendJoin(" | ", Expressions).Append(" ;").ToString();
    }

    /// <inheritdoc/>
    /// <remarks>Equaity is based on the nonterminal only.</remarks>
    public override bool Equals(object? obj) => obj is Rule rule
                                                && EqualityComparer<Nonterminal>.Default.Equals(Nonterminal, rule.Nonterminal);

    /// <inheritdoc/>
    /// <remarks>Equaity is based on the nonterminal only.</remarks>
    public override int GetHashCode()
    {
        return HashCode.Combine(Nonterminal);
    }
}
