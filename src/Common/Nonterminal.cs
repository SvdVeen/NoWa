namespace NoWa.Common;

/// <summary>
/// Represents a nonterminal symbol.
/// </summary>
public class Nonterminal : ISymbol
{
    private string _value;

    /// <summary>
    /// Gets or sets the value of the nonterminal.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when an empty value is passed.</exception>
    public string Value
    {
        get => _value;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "Cannot give an empty value to a nonterminal.");
            }
            _value = value;
        }
    }

    /// <summary>
    /// Construct a new nonterminal with the given value.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <exception cref="ArgumentNullException">Thrown when an empty <paramref name="value"/> is passed.</exception>
    public Nonterminal(string value)
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
}
