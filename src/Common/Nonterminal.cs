using System.Collections.Immutable;

namespace NoWa.Common;

/// <summary>
/// Represents a nonterminal symbol.
/// </summary>
public class Nonterminal : ISymbol
{
    #region Flyweight
    private static readonly Dictionary<string, Nonterminal> _nonterminals = new();

    /// <summary>
    /// Gets an instance of the <see cref="Nonterminal"/> class with the given value.
    /// </summary>
    /// <param name="value">The value to get a nonterminal for.</param>
    /// <returns>A shared instance of a <see cref="Nonterminal"/> with the given value.</returns>
    /// <exception cref="ArgumentNullException">The value passed is empty or whitespace.</exception>
    public static Nonterminal Get(string value)
    {
        if (!_nonterminals.TryGetValue(value, out Nonterminal? nonterminal))
        {
            nonterminal = new Nonterminal(value);
            _nonterminals[value] = nonterminal;
        }
        return nonterminal!;
    }
    #endregion Flyweight

    private readonly string _value;

    /// <summary>
    /// Gets the value of the nonterminal.
    /// </summary>
    public string Value { get => _value; }
    
    /// <summary>
    /// Construct a new nonterminal with the given value.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <exception cref="ArgumentNullException">Thrown when an empty <paramref name="value"/> is passed.</exception>
    private Nonterminal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Cannot give an empty value to a nonterminal.");
        }
        _value = value;
    }

    /// <inheritdoc/>
    public override string ToString() => Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Nonterminal nonterminal
                                                && Value == nonterminal.Value;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(Nonterminal? left, Nonterminal? right)
    {
        return EqualityComparer<Nonterminal>.Default.Equals(left, right);
    }

    public static bool operator !=(Nonterminal? left, Nonterminal? right)
    {
        return !(left == right);
    }
}
