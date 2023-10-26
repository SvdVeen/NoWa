using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free weighted attribute grammar.
/// </summary>
public class WAG : Grammar
{
    /// <inheritdoc/>
    public override void AddProduction(Production production)
    {
        if (production.Weight.Get() == null)
        {
            production.Weight.Set(1);
        }
        base.AddProduction(production);
    }

    #region ToString
    /// <inheritdoc/>
    public override string ToString()
    {
        if (_productions.Count == 0)
        {
            return "Empty WAG";
        }

        StringBuilder sb = new();
        for (int i = 0; i < _productions.Count; i++)
        {
            sb.AppendLine(ProductionToString(_productions[i]));
        }
        return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }

    /// <summary>
    /// Gets a string representation of a production in the WAG.
    /// </summary>
    /// <param name="production">The production to get a representation of.</param>
    /// <returns>A formatted string representing the production.</returns>
    private static string ProductionToString(Production production)
        => $"{HeadToString(production.Head)} -{production.Weight}-> {BodyToString(production.Body)} ;";

    /// <summary>
    /// Gets a string representation of the head of a production.
    /// </summary>
    /// <param name="head">The head to get a representation of.</param>
    /// <returns>A formatted string representing the head.</returns>
    private static string HeadToString(Nonterminal head)
    {
        if (head.InheritedAttributes.Count == 0 && head.SynthesizedAttributes.Count == 0)
        {
            return head.ToString();
        }

        return new StringBuilder(head.Value)
            .Append('{')
            .AppendJoin(',', head.InheritedAttributes)
            .Append(';')
            .AppendJoin(',', head.SynthesizedAttributes)
            .Append('}')
            .ToString();
    }

    /// <summary>
    /// Gets a string representation of the body of a production in the WAG.
    /// </summary>
    /// <param name="body">The body to get a representation of.</param>
    /// <returns>A formatted string representing the production.</returns>
    private static string BodyToString(IEnumerable<ISymbol> body) => new StringBuilder().AppendJoin(' ', body.Select(SymbolToString)).ToString();

    /// <summary>
    /// Gets a string representation of a symbol in the body of a production in the WAG.
    /// </summary>
    /// <param name="symbol">The symbol to get a representation of.</param>
    /// <returns>A formatted string representing the symbol.</returns>
    private static string SymbolToString(ISymbol symbol)
    {
        if (symbol is Nonterminal nonterminal)
        {
            if (nonterminal.StaticAttributes.Count == 0)
            {
                return nonterminal.ToString();
            }

            return new StringBuilder(nonterminal.Value)
            .Append('{')
            .AppendJoin(',', nonterminal.StaticAttributes)
            .Append('}')
            .ToString();
        }
        return symbol.ToString()!;
    }
    #endregion ToString
}
