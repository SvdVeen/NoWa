using System.Collections.Immutable;
using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free weighted attribute grammar.
/// </summary>
public class WAG : Grammar
{
    #region Attributes
    /// <summary>
    /// Helper function for creating a set of attributes if it does not exist yet.
    /// </summary>
    /// <param name="attrdict">The internal dictionary of attributes to get a nonterminal's attributes from.</param>
    /// <param name="nonterminal">The nonterminal to get attributes for.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">The given nonterminal is not part of the grammar.</exception>
    private HashSet<char> GetOrCreateAttributesSet(Dictionary<Nonterminal, HashSet<char>> attrdict, Nonterminal nonterminal)
    {
        _ = AddNonterminal(nonterminal);
        if (!attrdict.TryGetValue(nonterminal, out HashSet<char>? attributes))
        {
            attrdict.Add(nonterminal, attributes = new HashSet<char>());
        }
        return attributes;
    }

    #region Inherited
    private readonly Dictionary<Nonterminal, HashSet<char>> _inheritedattributes = new();

    /// <summary>
    /// Gets the set of inherited attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get inherited attributes for.</param>
    /// <returns>The set of inherited attributes for the given nonterminal.</returns>
    private HashSet<char> GetInheritedAttributesInternal(Nonterminal nonterminal)
        => GetOrCreateAttributesSet(_inheritedattributes, nonterminal);

    /// <summary>
    /// Gets the set of inherited attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get the inherited attributes for.</param>
    /// <returns>The set of inherited attributes for the given nonterminal.</returns>
    public IReadOnlySet<char> GetInheritedAttributes(Nonterminal nonterminal)
    {
        return GetInheritedAttributesInternal(nonterminal);
    }

    /// <summary>
    /// Add an inherited attribute for a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add an inherited attribute for.</param>
    /// <param name="attribute">The attribute to add.</param>
    /// <returns><see langword="true"/> if the attribute could be added, <see langword="false"/> if it already existed for this nonterminal.</returns>
    public bool AddInheritedAttribute(Nonterminal nonterminal, char attribute)
    {
        HashSet<char> attributes = GetInheritedAttributesInternal(nonterminal);
        return
            attributes.Add(attribute);
    }

    /// <summary>
    /// Remove an inherited attribute for a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to remove an inherited attribute for.</param>
    /// <param name="attribute">The attribute to remove.</param>
    /// <returns><see langword="true"/> if the attribute could be removed, <see langword="false"/> if the nonterminal did not have this attribute.</returns>
    public bool RemoveInheritedAttribute(Nonterminal nonterminal, char attribute)
    {
        HashSet<char> attributes = GetInheritedAttributesInternal(nonterminal);
        return attributes.Remove(attribute);
    }
    #endregion Inherited

    #region Synthesized
    private readonly Dictionary<Nonterminal, HashSet<char>> _synthesizedattributes = new();

    /// <summary>
    /// Gets the set of synthesized attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get synthesized attributes for.</param>
    /// <returns>The set of synthesized attributes for the given nonterminal.</returns>
    private HashSet<char> GetSynthesizedAttributesInternal(Nonterminal nonterminal)
        => GetOrCreateAttributesSet(_synthesizedattributes, nonterminal);

    /// <summary>
    /// Gets the set of synthesized attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get the synthesized attributes for.</param>
    /// <returns>The set of synthesized attributes for the given nonterminal.</returns>
    public IReadOnlySet<char> GetSynthesizedAttributes(Nonterminal nonterminal)
    {
        return GetSynthesizedAttributesInternal(nonterminal);
    }

    /// <summary>
    /// Add a synthesized attribute for a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add a synthesized attribute for.</param>
    /// <param name="attribute">The attribute to add.</param>
    /// <returns><see langword="true"/> if the attribute could be added, <see langword="false"/> if it already existed for this nonterminal.</returns>
    public bool AddSynthesizedAttribute(Nonterminal nonterminal, char attribute)
    {
        HashSet<char> attributes = GetSynthesizedAttributesInternal(nonterminal);
        return attributes.Add(attribute);
    }

    /// <summary>
    /// Remove a synthesized attribute for a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to remove a synthesized attribute for.</param>
    /// <param name="attribute">The attribute to remove.</param>
    /// <returns><see langword="true"/> if the attribute could be removed, <see langword="false"/> if the nonterminal did not have this attribute.</returns>
    public bool RemoveSynthesizedAttribute(Nonterminal nonterminal, char attribute)
    {
        HashSet<char> attributes = GetSynthesizedAttributesInternal(nonterminal);
        return attributes.Remove(attribute);
    }

    #endregion Synthesized

    #region Static
    private readonly Dictionary<Nonterminal, HashSet<char>> _staticattributes = new();

    /// <summary>
    /// Gets the set of static attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The static to get synthesized attributes for.</param>
    /// <returns>The set of static attributes for the given nonterminal.</returns>
    private HashSet<char> GetStaticAttributesInternal(Nonterminal nonterminal)
        => GetOrCreateAttributesSet(_staticattributes, nonterminal);

    /// <summary>
    /// Gets the set of static attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get the static attributes for.</param>
    /// <returns>The set of static attributes for the given nonterminal.</returns>
    public IReadOnlySet<char> GetStaticAttributes(Nonterminal nonterminal)
    {
        return GetStaticAttributesInternal(nonterminal);
    }

    /// <summary>
    /// Add a static attribute for a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add a static attribute for.</param>
    /// <param name="attribute">The attribute to add.</param>
    /// <returns><see langword="true"/> if the attribute could be added, <see langword="false"/> if it already existed for this nonterminal.</returns>
    public bool AddStaticAttribute(Nonterminal nonterminal, char attribute)
    {
        HashSet<char> attributes = GetStaticAttributesInternal(nonterminal);
        return attributes.Add(attribute);
    }

    /// <summary>
    /// Remove a static attribute for a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to remove a static attribute for.</param>
    /// <param name="attribute">The attribute to remove.</param>
    /// <returns><see langword="true"/> if the attribute could be removed, <see langword="false"/> if the nonterminal did not have this attribute.</returns>
    public bool RemoveStaticAttribute(Nonterminal nonterminal, char attribute)
    {
        HashSet<char> attributes = GetStaticAttributesInternal(nonterminal);
        return attributes.Remove(attribute);
    }

    #endregion Static

    /// <summary>
    /// Get the set of all attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get attributes for.</param>
    /// <returns>The set of all attributes for a nonterminal.</returns>
    public IReadOnlySet<char> GetAllAttributes(Nonterminal nonterminal)
    {
        return GetInheritedAttributes(nonterminal)
            .Union(GetSynthesizedAttributes(nonterminal))
            .Union(GetStaticAttributesInternal(nonterminal))
            .ToImmutableSortedSet();
    }
    #endregion Attributes

    /// <inheritdoc/>
    public override void AddProduction(Production production)
    {
        if (production.Weight.Get() == null)
        {
            production.Weight.Set(1);
        }
        base.AddProduction(production);
    }

    /// <inheritdoc/>
    public override void RemoveNonterminalAt(int index)
    {
        if (index < 0 || index > Nonterminals.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Nonterminal nt = Nonterminals[index];
        _inheritedattributes.Remove(nt);
        _synthesizedattributes.Remove(nt);
        _staticattributes.Remove(nt);
        base.RemoveNonterminalAt(index);
    }

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        _inheritedattributes.Clear();
        _synthesizedattributes.Clear();
        _staticattributes.Clear();
    }

    public override Grammar Clone()
    {
        WAG clone = new WAG();
        CloneTo(clone);
        foreach (var pair in _inheritedattributes)
        {
            foreach (char attr in pair.Value)
            {
                AddInheritedAttribute(pair.Key, attr);
            }
        }
        foreach (var pair in _synthesizedattributes)
        {
            foreach (char attr in pair.Value)
            {
                AddSynthesizedAttribute(pair.Key, attr);
            }
        }
        foreach (var pair in _staticattributes)
        {
            foreach (char attr in pair.Value)
            {
                AddStaticAttribute(pair.Key, attr);
            }
        }
        return clone;
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
    private string ProductionToString(Production production)
        => $"{HeadToString(production.Head)} -{production.Weight}-> {BodyToString(production.Body)}{ExprsToString(production.Expressions)} ;";

    /// <summary>
    /// Gets a string representation of the head of a production.
    /// </summary>
    /// <param name="head">The head to get a representation of.</param>
    /// <returns>A formatted string representing the head.</returns>
    private string HeadToString(Nonterminal head)
    {

        if (GetInheritedAttributesInternal(head).Count == 0
            && GetSynthesizedAttributesInternal(head).Count == 0
            && GetStaticAttributesInternal(head).Count == 0)
        {
            return head.ToString();
        }

        return new StringBuilder(head.Value)
            .Append('{')
            .AppendJoin(',', GetInheritedAttributesInternal(head))
            .Append(';')
            .AppendJoin(',', GetSynthesizedAttributesInternal(head))
            .Append(';')
            .AppendJoin(',', GetStaticAttributesInternal(head))
            .Append('}')
            .ToString();
    }

    /// <summary>
    /// Gets a string representation of the body of a production in the WAG.
    /// </summary>
    /// <param name="body">The body to get a representation of.</param>
    /// <returns>A formatted string representing the production.</returns>
    private string BodyToString(IEnumerable<ISymbol> body) => new StringBuilder().AppendJoin(' ', body.Select(SymbolToString)).ToString();

    private string ExprsToString(IList<Expressions.Expression> exprs)
    {
        if (exprs.Count > 0)
        {
            return new StringBuilder(" (").AppendJoin(',', exprs).Append(')').ToString();
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets a string representation of a symbol in the body of a production in the WAG.
    /// </summary>
    /// <param name="symbol">The symbol to get a representation of.</param>
    /// <returns>A formatted string representing the symbol.</returns>
    private string SymbolToString(ISymbol symbol)
    {
        if (symbol is Nonterminal nonterminal)
        {
            var allAttrs = GetAllAttributes(nonterminal);
            if (allAttrs.Count == 0)
            {
                return nonterminal.ToString();
            }

            return new StringBuilder(nonterminal.Value)
            .Append('{')
            .AppendJoin(',', allAttrs)
            .Append('}')
            .ToString();
        }
        return symbol.ToString()!;
    }
    #endregion ToString
}
