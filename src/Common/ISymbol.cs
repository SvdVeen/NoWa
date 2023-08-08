namespace NoWa.Common;

/// <summary>
/// Interface for symbols. Could be a terminal or a nonterminal.
/// </summary>
public interface ISymbol
{
    /// <summary>
    /// The value of the symbol.
    /// </summary>
    public string Value { get; }
}
