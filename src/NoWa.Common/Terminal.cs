namespace NoWa.Common;

/// <summary>
/// Represents a terminal symbol in a grammar.
/// </summary>
public class Terminal : ISymbol
{
    /// <summary>
    /// Gets or sets the value of the terminal.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Construct a new terminal with the given value.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    public Terminal(string value) => Value = value;
    
    /// <inheritdoc/>
    public override string ToString() => $"\'{Value}\'";
}
