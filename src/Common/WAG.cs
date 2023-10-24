using System.Globalization;
using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free weighted attribute grammar.
/// </summary>
public class WAG : CFG
{
    #region Weights
    private readonly List<double> _weights = new();

    /// <summary>
    /// Gets the weights in the WAG.
    /// </summary>
    public IReadOnlyList<double> Weights { get => _weights; }

    /// <summary>
    /// Adds a production to the grammar and gives it a default weight of one.
    /// Also automatically updates the terminals and nonterminals in the grammar.
    /// </summary>
    /// <param name="production">The production to add.</param>
    public override void AddProduction(Production production)
    {
        AddProduction(production, 1);
    }

    /// <summary>
    /// Adds a production to the grammar and assigns the given weight.
    /// </summary>
    /// <param name="production">The production to add.</param>
    /// <param name="weight">The weight of the production.</param>
    public void AddProduction(Production production, double weight)
    {
        base.AddProduction(production);
        _weights.Add(weight);
    }

    /// <summary>
    /// Gets the weight of the given production.
    /// </summary>
    /// <param name="production"></param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="production"/> does not have an entry in in <see cref="_weights"/>.</exception>
    public double GetWeight(Production production)
    {
        int index = _productions.IndexOf(production);
        if (index < 0 || index >= _weights.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(production));
        }
        return _weights[index];
    }

    /// <inheritdoc/>
    public override bool RemoveProduction(Production production)
    {
        int index = _productions.IndexOf(production);
        if (index > -1)
        {
            if (index >= _weights.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(production));
            }
            _weights.RemoveAt(index);
            return base.RemoveProduction(production);

        }
        return false;
    }

    /// <inheritdoc/>
    public override void RemoveProductionAt(int index)
    {
        if (index < 0 || index >= _productions.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        if (index >= _weights.Count)
        {
            throw new InvalidOperationException($"Production {index} does not have an assigned weight.");
        }
        _weights.RemoveAt(index);
        base.RemoveProductionAt(index);
    }

    /// <summary>
    /// Sets the weight of a particular production.
    /// </summary>
    /// <param name="production">The production to set the weight for.</param>
    /// <param name="weight">The weight to assign to the production.</param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="production"/> does not have an entry in <see cref="_weights"/>.</exception>
    public void SetWeight(Production production, double weight)
    {
        int index = _productions.IndexOf(production);
        if (index < 0 || index >= _weights.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(production));
        }
        _weights[index] = weight;
    }
    #endregion Weights

    #region Attributes
    private readonly Dictionary<Nonterminal, HashSet<char>?> _inheritedAttributes = new();
    private readonly Dictionary<Nonterminal, HashSet<char>?> _synthesizedAttributes = new();

    /// <inheritdoc/>
    public override bool AddNonterminal(Nonterminal nonterminal)
    {
        if (base.AddNonterminal(nonterminal))
        {
            _inheritedAttributes.Add(nonterminal, new HashSet<char>());
            _synthesizedAttributes.Add(nonterminal, new HashSet<char>());
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds an inherited attribute to a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add the attribute to.</param>
    /// <param name="attribute">The attribute to add.</param>
    /// <returns><see langword="true"/> if the attribute was added (it did not exist yet for this nonterminal), <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException">The <paramref name="nonterminal"/> did not exist in the grammar.</exception>
    public bool AddInheritedAttribute(Nonterminal nonterminal, char attribute)
    {
        if (!_inheritedAttributes.TryGetValue(nonterminal, out HashSet<char>? attrs) || attrs == null)
        {
            throw new ArgumentException($"The grammar does not contain the nonterminal {nonterminal}.", nameof(nonterminal));
        }
        return attrs!.Add(attribute);
    }

    /// <summary>
    /// Adds a synthesized attribute to a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add the attribute to.</param>
    /// <param name="attribute">The attribute to add.</param>
    /// <returns><see langword="true"/> if the attribute was added (it did not exist yet for this nonterminal), <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentException">The <paramref name="nonterminal"/> did not exist in the grammar.</exception>
    public bool AddSynthesizedAttribute(Nonterminal nonterminal, char attribute)
    {
        if (!_synthesizedAttributes.TryGetValue(nonterminal, out HashSet<char>? attrs) || attrs == null)
        {
            throw new ArgumentException($"The grammar does not contain the nonterminal {nonterminal}.", nameof(nonterminal));
        }
        return attrs!.Add(attribute);
    }

    /// <summary>
    /// Gets the inherited attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get attributes for.</param>
    /// <returns>The set of inherited attributes for the given nonterminal.</returns>
    public IReadOnlySet<char> GetInheritedAttributes(Nonterminal nonterminal)
    {
        if (!_inheritedAttributes.TryGetValue(nonterminal, out HashSet<char>? attrs) || attrs == null)
        {
            attrs = new HashSet<char>(0);
        }
        return attrs;
    }

    /// <summary>
    /// Gets the synthesized attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get attributes for.</param>
    /// <returns>The set of synthesized attributes for the given nonterminal.</returns>
    public IReadOnlySet<char> GetSynthesizedAttributes(Nonterminal nonterminal)
    {
        if (!_synthesizedAttributes.TryGetValue(nonterminal, out HashSet<char>? attrs) || attrs == null)
        {
            attrs = new HashSet<char> (0);
        }
        return attrs;
    }
    
    /// <summary>
    /// Gets the static attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get attributes for.</param>
    /// <returns>The set of static attributes for the given nonterminal.</returns>
    public IReadOnlySet<char> GetStaticAttributes(Nonterminal nonterminal)
    {
        var inherited = GetInheritedAttributes(nonterminal);
        var synthesized = GetSynthesizedAttributes(nonterminal);
        // If we have an attribute with the same name that is both inherited and synthesized, it is lost here.
        // Should that be allowed to begin with?
        return inherited.Union(synthesized).ToHashSet();
    }

    /// <summary>
    /// Removes an inherited attribute from a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to remove the attribute from.</param>
    /// <param name="attribute">The attribute to remove.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">The given <paramref name="nonterminal"/> does not exist in the grammar.</exception>
    public bool RemoveInheritedAttribute(Nonterminal nonterminal, char attribute)
    {
        if (!_inheritedAttributes.TryGetValue(nonterminal, out HashSet<char>? attrs) || attrs == null)
        {
            throw new ArgumentException($"The grammar does not contain the nonterminal {nonterminal}.", nameof(nonterminal));
        }
        return attrs.Remove(attribute);
    }

    /// <summary>
    /// Removes a synthesized attribute from a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to remove the attribute from.</param>
    /// <param name="attribute">The attribute to remove.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">The given <paramref name="nonterminal"/> does not exist in the grammar.</exception>
    public bool RemoveSynthesizedAttribute(Nonterminal nonterminal, char attribute)
    {
        if (!_synthesizedAttributes.TryGetValue(nonterminal, out HashSet<char>? attrs) || attrs == null)
        {
            throw new ArgumentException($"The grammar does not contain the nonterminal {nonterminal}.", nameof(nonterminal));
        }
        return attrs.Remove(attribute);
    }

    /// <inheritdoc/>
    public override void RemoveNonterminalAt(int index)
    {
        if (index < 0 ||  index >= _inheritedAttributes.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Nonterminal nonterminal = _nonterminalsList[index];
        _ = _inheritedAttributes.Remove(nonterminal);
        _ = _synthesizedAttributes.Remove(nonterminal);
        base.RemoveNonterminalAt(index);
    }
    #endregion Attributes

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        _inheritedAttributes.Clear();
        _synthesizedAttributes.Clear();
        _weights.Clear();
    }

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
            sb.AppendLine(ProductionToString(_productions[i], _weights[i]));
        }
        return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }

    /// <summary>
    /// Gets a string representation of a production in the WAG.
    /// </summary>
    /// <param name="production">The production to get a representation of.</param>
    /// <returns>A formatted string representing the production.</returns>
    private string ProductionToString(Production production, double weight) => $"{HeadToString(production.Head)} -{weight.ToString(CultureInfo.InvariantCulture)}-> {BodyToString(production.Body)} ;";

    /// <summary>
    /// Gets a string representation of the head of a production.
    /// </summary>
    /// <param name="head">The head to get a representation of.</param>
    /// <returns>A formatted string representing the head.</returns>
    private string HeadToString(Nonterminal head)
    {
        var inheritedAttributes = GetInheritedAttributes(head);
        var synthesizedAttributes = GetSynthesizedAttributes(head);
        if (inheritedAttributes.Count == 0 && synthesizedAttributes.Count == 0)
        {
            return head.ToString();
        }
        else
        {
            return new StringBuilder($"{head}{{")
                .AppendJoin(',', inheritedAttributes)
                .Append(';')
                .AppendJoin(',', synthesizedAttributes)
                .Append('}')
                .ToString();
        }
    }

    /// <summary>
    /// Gets a string representation of the body of a production in the WAG.
    /// </summary>
    /// <param name="body">The body to get a representation of.</param>
    /// <returns>A formatted string representing the production.</returns>
    private string BodyToString(IEnumerable<ISymbol> body) => new StringBuilder().AppendJoin(' ', body.Select(SymbolToString)).ToString();

    /// <summary>
    /// Gets a string representation of a symbol.
    /// </summary>
    /// <param name="symbol">The symbol to get a representation of.</param>
    /// <returns>A formatted string representing the symbol.</returns>
    private string SymbolToString(ISymbol symbol)
    {
        if (symbol is Nonterminal nonterminal)
        {
            return NonterminalToString(nonterminal);
        }
        return symbol.ToString()!;
    }

    /// <summary>
    /// Gets a string representation of a nonterminal in the body of a production in the WAG.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get a representation of.</param>
    /// <returns>A formatted string representing the nonterminal.</returns>
    private string NonterminalToString(Nonterminal nonterminal)
    {
        var attributes = GetStaticAttributes(nonterminal);
        if (attributes.Count == 0)
        {
            return nonterminal.ToString();
        }
        else
        {
            return $"{nonterminal}{{{new StringBuilder().AppendJoin(',', attributes)}}}";
        }
    }
}
