namespace NoWa.Common;

/// <summary>
/// Represents a terminal symbol in a grammar.
/// </summary>
public class Terminal : ISymbol
{
    #region Flyweight
    private static readonly Dictionary<string, Terminal> _terminals = new();

    /// <summary>
    /// Gets an instance of the <see cref="Terminal"/> class with the given value.
    /// </summary>
    /// <param name="value">The value to get a terminal for.</param>
    /// <returns>A shared instance of a <see cref="Terminal"/> with the given value.</returns>
    /// <exception cref="ArgumentNullException">The value passed is empty or whitespace.</exception>
    public static Terminal Get(string value)
    {
        if (!_terminals.TryGetValue(value, out Terminal? terminal))
        {
            terminal = new Terminal(value);
            _terminals[value] = terminal;
        }
        return terminal!;
    }
    #endregion Flyweight

    private readonly string _value;

    /// <summary>
    /// Gets the value of the terminal.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when an empty value is passed.</exception>
    public string Value { get => _value; }

    /// <summary>
    /// Construct a new terminal with the given value.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <exception cref="ArgumentNullException">Thrown when an empty value is passed.</exception>
    private Terminal(string value)
    {
        if (string.IsNullOrEmpty(value))
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

    public static bool operator ==(Terminal? left, Terminal? right)
    {
        return EqualityComparer<Terminal>.Default.Equals(left, right);
    }

    public static bool operator !=(Terminal? left, Terminal? right)
    {
        return !(left == right);
    }
}
