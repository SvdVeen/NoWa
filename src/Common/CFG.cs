using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free grammar.
/// </summary>
public class CFG : Grammar
{
    /// <inheritdoc/>
    public override Grammar Clone()
    {
        CFG clone = new();
        CloneTo(clone);
        return clone;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_productions.Count == 0)
        {
            return "Empty grammar";
        }

        return new StringBuilder().AppendJoin(Environment.NewLine, _productionsByHead.Select(ProductionsToString)).ToString();
    }

    /// <summary>
    /// Shorthand for converting entries in <see cref="_productionsByHead"/> to a string.
    /// </summary>
    /// <param name="group">A pair of a nonterminal and all productions with it as their heads.</param>
    /// <returns>A formatted string for displaying all productions of a nonterminal.</returns>
    private static string ProductionsToString(KeyValuePair<Nonterminal, List<Production>> group)
    {
        return new StringBuilder($"{group.Key} = ").AppendJoin(" | ", group.Value.Select(p => BodyToString(p.Body))).Append(" ;").ToString();
    }

    /// <summary>
    /// Shorthand for converting a production body to a string.
    /// </summary>
    /// <param name="body">The body to get the string representation of.</param>
    /// <returns>A formatted string for displaying the production body.</returns>
    private static string BodyToString(IEnumerable<ISymbol> body)
    {
        return new StringBuilder().AppendJoin(' ', body).ToString();
    }
}
