namespace NoWa.Common;

/// <summary>
/// Represents a nonterminal symbol.
/// </summary>
public class Nonterminal : ISymbol
{
    /// <summary>
    /// Gets or sets the value of the nonterminal.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Construct a new nonterminal with the given value.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    public Nonterminal(string value) => Value = value;

    /// <inheritdoc/>
    public override string ToString() => Value;
}
