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
    private readonly HashSet<char> _inheritedAttributes = new();
    private readonly Dictionary<Nonterminal, HashSet<char>> _inheritedAttributesByNonterminal = new();

    /// <summary>
    /// Gets the set of inherited attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get inherited attributes for.</param>
    /// <returns>The set of inherited attributes for the given nonterminal.</returns>
    private HashSet<char> GetInheritedAttributesInternal(Nonterminal nonterminal)
        => GetOrCreateAttributesSet(_inheritedAttributesByNonterminal, nonterminal);

    /// <summary>
    /// Gets the set of all inherited attributes.
    /// </summary>
    /// <returns>The set of all inherited attributes.</returns>
    public IReadOnlySet<char> GetInheritedAttributes() => _inheritedAttributes;

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
        if (!_synthesizedAttributes.Contains(attribute) && !_staticAttributes.Contains(attribute))
        {
            HashSet<char> attributes = GetInheritedAttributesInternal(nonterminal);
            _ = _inheritedAttributes.Add(attribute);
            return attributes.Add(attribute);
        }
        return false;
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
        bool result = attributes.Remove(attribute);
        if (!_inheritedAttributesByNonterminal.Any(p => p.Value.Contains(attribute)))
        {
            _ = _inheritedAttributes.Remove(attribute);
        }
        return result;
    }
    #endregion Inherited

    #region Synthesized
    private readonly HashSet<char> _synthesizedAttributes = new();
    private readonly Dictionary<Nonterminal, HashSet<char>> _synthesizedAttributesByNonterminal = new();

    /// <summary>
    /// Gets the set of synthesized attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get synthesized attributes for.</param>
    /// <returns>The set of synthesized attributes for the given nonterminal.</returns>
    private HashSet<char> GetSynthesizedAttributesInternal(Nonterminal nonterminal)
        => GetOrCreateAttributesSet(_synthesizedAttributesByNonterminal, nonterminal);

    /// <summary>
    /// Gets the set of all synthesized attributes.
    /// </summary>
    /// <returns>The set of all synthesized attributes.</returns>
    public IReadOnlySet<char> GetSynthesizedAttributes() => _synthesizedAttributes;

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
        if (!_inheritedAttributes.Contains(attribute) && !_staticAttributes.Contains(attribute))
        {
            HashSet<char> attributes = GetSynthesizedAttributesInternal(nonterminal);
            _ = _synthesizedAttributes.Add(attribute);
            return attributes.Add(attribute);
        }
        return false;
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
        bool result = attributes.Remove(attribute);
        if (!_synthesizedAttributesByNonterminal.Any(p => p.Value.Contains(attribute)))
        {
            _ = _synthesizedAttributes.Remove(attribute);
        }
        return result;
    }

    #endregion Synthesized

    #region Static
    private readonly HashSet<char> _staticAttributes = new();
    private readonly Dictionary<Nonterminal, HashSet<char>> _staticAttributesByNonterminal = new();

    /// <summary>
    /// Gets the set of static attributes for a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The static to get synthesized attributes for.</param>
    /// <returns>The set of static attributes for the given nonterminal.</returns>
    private HashSet<char> GetStaticAttributesInternal(Nonterminal nonterminal)
        => GetOrCreateAttributesSet(_staticAttributesByNonterminal, nonterminal);

    /// <summary>
    /// Gets the set of all static attributes.
    /// </summary>
    /// <returns>The set of all static attributes.</returns>
    public IReadOnlySet<char> GetStaticAttributes() => _staticAttributes;

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
        if (!_inheritedAttributes.Contains(attribute) && !_synthesizedAttributes.Contains(attribute))
        {
            HashSet<char> attributes = GetStaticAttributesInternal(nonterminal);
            _ = _staticAttributes.Add(attribute);
            return attributes.Add(attribute);
        }
        return false;
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
        bool result = attributes.Remove(attribute);
        if (!_staticAttributesByNonterminal.Any(p => p.Value.Contains(attribute)))
        {
            _ = _staticAttributes.Remove(attribute);
        }
        return result;
    }

    #endregion Static

    /// <summary>
    /// Gets the set of all attributes.
    /// </summary>
    /// <returns>The set of all attributes.</returns>
    public IReadOnlySet<char> GetAllAttributes()
    {
        return _inheritedAttributes
            .Union(_synthesizedAttributes)
            .Union(_staticAttributes)
            .ToImmutableSortedSet();
    }

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
        _inheritedAttributesByNonterminal.Remove(nt);
        _synthesizedAttributesByNonterminal.Remove(nt);
        _staticAttributesByNonterminal.Remove(nt);
        base.RemoveNonterminalAt(index);
    }

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        _inheritedAttributesByNonterminal.Clear();
        _synthesizedAttributesByNonterminal.Clear();
        _staticAttributesByNonterminal.Clear();
    }

    public override Grammar Clone()
    {
        WAG clone = new WAG();
        CloneTo(clone);
        foreach (var pair in _inheritedAttributesByNonterminal)
        {
            foreach (char attr in pair.Value)
            {
                clone.AddInheritedAttribute(pair.Key, attr);
            }
        }
        foreach (var pair in _synthesizedAttributesByNonterminal)
        {
            foreach (char attr in pair.Value)
            {
                clone.AddSynthesizedAttribute(pair.Key, attr);
            }
        }
        foreach (var pair in _staticAttributesByNonterminal)
        {
            foreach (char attr in pair.Value)
            {
                clone.AddStaticAttribute(pair.Key, attr);
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

    /// <summary>
    /// Converts a list of expressions to a string.
    /// </summary>
    /// <param name="exprs">The expressions to convert to a string.</param>
    /// <returns>The comma-separated list of expressions in parentheses, or an empty string if there are none.</returns>
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
