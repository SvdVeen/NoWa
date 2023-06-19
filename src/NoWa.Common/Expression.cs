using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a combination of symbols.
/// </summary>
public class Expression
{
    /// <summary>
    /// The list of symbols in the expression.
    /// </summary>
    public IList<ISymbol> Symbols { get; } = new List<ISymbol>();

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"></exception>
    public override string ToString()
    {
        if (Symbols.Count == 0)
            throw new InvalidOperationException("Expression contains no symbols.");

        StringBuilder sb = new StringBuilder();
        _ = sb.Append($"{Symbols[0]}");
        for (int i = 1; i < Symbols.Count; i++)
            _ = sb.Append($" {Symbols[i]}");
        return sb.ToString();
    }
}
