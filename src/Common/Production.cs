using System.Text;

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
    public IList<ISymbol> Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    private Production(Nonterminal head, List<ISymbol> body)
    {
        Head = head;
        Body = body;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, IEnumerable<ISymbol> body) : this(head, new List<ISymbol>(body)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, params ISymbol[] body) : this(head, new List<ISymbol>(body)) { }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Body.Count == 0)
        {
            return $"Empty production {Head} ;";
        }
        return new StringBuilder($"{Head} --> ").AppendJoin(' ', Body).Append(" ;").ToString();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Production production &&
               EqualityComparer<Nonterminal>.Default.Equals(Head, production.Head) &&
               Body.SequenceEqual(production.Body);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Head, GetBodyHashCode());
    }

    /// <summary>
    /// Gets a hashcode for the <see cref="Body"/> that matches a combination of all elements.
    /// </summary>
    /// <returns></returns>
    private int GetBodyHashCode()
    {
        int result = 17;
        foreach (var symbol in Body)
        {
            result = HashCode.Combine(result, symbol);
        }
        return result;
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
