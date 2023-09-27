namespace NoWa.Common;

/// <summary>
/// Represents a production rule in a grammar.
/// </summary>
public class Production
{
    /// <summary>
    /// Gets the head of the production rule.
    /// </summary>
    public Nonterminal Head { get; }

    /// <summary>
    /// Gets the body of the production rule.
    /// </summary>
    public Expression Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, Expression body)
    {
        Head = head;
        Body = body;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, IEnumerable<ISymbol> body) : this(head, new Expression(body)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, params ISymbol[] body) : this(head, new Expression(body)) { }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Body.Count == 0)
        {
            return $"Empty production {Head} ;";
        }
        return $"{Head} --> {Body} ;";
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Production production &&
               EqualityComparer<Nonterminal>.Default.Equals(Head, production.Head) &&
               EqualityComparer<Expression>.Default.Equals(Body, production.Body);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Head, Body);
    }

    public static bool operator ==(Production? left, Production? right)
    {
        return EqualityComparer<Production>.Default.Equals(left, right);
    }

    public static bool operator !=(Production? left, Production? right)
    {
        return !(left == right);
    }
}
