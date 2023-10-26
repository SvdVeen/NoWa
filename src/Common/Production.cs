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
    /// Gets the weight of the production rule.
    /// </summary>
    public Weight Weight { get; }

    /// <summary>
    /// Gets the body of the production rule.
    /// </summary>
    public IList<ISymbol> Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    private Production(Nonterminal head, Weight weight, List<ISymbol> body)
    {
        Head = head;
        Body = body;
        Weight = weight;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, Weight weight, IEnumerable<ISymbol> body) : this(head, weight, new List<ISymbol>(body)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, IEnumerable<ISymbol> body) : this(head, new Weight(), body) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, double weight, IEnumerable<ISymbol> body) : this(head, new Weight(weight), body) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, string weight, IEnumerable<ISymbol> body) : this(head, new Weight(weight), body) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, Weight weight, params ISymbol[] body) : this(head, weight, new List<ISymbol>(body)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, params ISymbol[] body) : this(head, new Weight(), body) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, double weight, params ISymbol[] body) : this(head, new Weight(weight), body) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Production"/> class.
    /// </summary>
    /// <param name="head">The head of the production rule.</param>
    /// <param name="weight">The weight of the production rule.</param>
    /// <param name="body">The body of the production rule.</param>
    public Production(Nonterminal head, string weight, params ISymbol[] body) : this(head, new Weight(weight), body) { }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Body.Count == 0)
        {
            return $"Empty production {Head} ;";
        }
        return new StringBuilder($"{Head} -{Weight}-> ").AppendJoin(' ', Body).Append(" ;").ToString();
    }

    /// <inheritdoc/>
    /// <remarks>Uses sequence equality on the body.</remarks>
    public override bool Equals(object? obj)
    {
        return obj is Production production &&
               EqualityComparer<Nonterminal>.Default.Equals(Head, production.Head) &&
               EqualityComparer<Weight>.Default.Equals(Weight, production.Weight) &&
               Body.SequenceEqual(production.Body);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Head, Weight, GetBodyHashCode());
    }

    /// <summary>
    /// Gets a hashcode for the <see cref="Body"/> that matches a combination of all elements.
    /// </summary>
    /// <returns>The hashcode for the entire body.</returns>
    /// <remarks>This overflows pretty quickly if the body is large, so it's better to avoid using it (yay).</remarks>
    private int GetBodyHashCode()
    {
        int result = 3;
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
