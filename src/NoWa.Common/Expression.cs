using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a combination of symbols.
/// </summary>
public class Expression
{
    /// <summary>
    /// Gets the list of symbols in the expression.
    /// </summary>
    public IList<ISymbol> Symbols { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="Expression"/> class.
    /// </summary>
    public Expression() {
        Symbols = new List<ISymbol>();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Expression"/> class.
    /// </summary>
    /// <param name="symbols">The symbols in this expression.</param>
    public Expression(params ISymbol[] symbols)
    {
        Symbols = new List<ISymbol>(symbols);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"></exception>
    public override string ToString()
    {
        if (Symbols.Count == 0)
            throw new InvalidOperationException("Expression contains no symbols.");

        StringBuilder sb = new();
        _ = sb.Append($"{Symbols[0]}");
        for (int i = 1; i < Symbols.Count; i++)
            _ = sb.Append($" {Symbols[i]}");
        return sb.ToString();
    }
}
