namespace NoWa.Common;

/// <summary>
/// Represents a terminal symbol in a grammar.
/// </summary>
public class Terminal : ISymbol
{
    private string _value;

    /// <summary>
    /// Gets or sets the value of the terminal.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when an empty value is passed.</exception>
    public string Value
    {
        get => _value;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), "Cannot give an empty value to a terminal.");
            }
            _value = value;
        }
    }

    /// <summary>
    /// Construct a new terminal with the given value.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <exception cref="ArgumentNullException">Thrown when an empty value is passed.</exception>
    public Terminal(string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value), "Cannot give an empty value to a terminal.");
        }
        _value = value;
    }

    /// <inheritdoc/>
    public override string ToString() => $"\'{Value}\'";

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Terminal terminal
                                                && Value == terminal.Value;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(ToString());
}
