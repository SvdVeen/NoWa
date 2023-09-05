using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free grammar.
/// </summary>
public class Grammar
{
    private readonly Dictionary<string, Nonterminal> _nonterminals = new();
    private readonly Dictionary<string, Terminal> _terminals = new();
    private readonly Dictionary<Nonterminal, Rule> _rulesByNonterminal = new();
    private readonly List<Rule> _rules = new();

    #region Rules
    /// <summary>
    /// Gets the number of rules in the grammar.
    /// </summary>
    public int RuleCount { get => _rules.Count; }

    /// <summary>
    /// Gets a rule with the given index.
    /// </summary>
    /// <param name="index">The index of the rule to get.</param>
    /// <returns>The rule with the given index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the bounds of the list of rules.</exception>
    public Rule GetRule(int index)
    {
        if (index < 0 || index >= _rules.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        return _rules[index];
    }

    /// <summary>
    /// Gets a rule with the given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal matching the rule.</param>
    /// <returns>The rule for the given nonterminal.</returns>
    /// <exception cref="ArgumentException">No rule with the given nonterminal value was found.</exception>
    public Rule GetRule(string nonterminal)
    {
        if (_nonterminals.TryGetValue(nonterminal, out Nonterminal? nt) && _rulesByNonterminal.TryGetValue(nt!, out Rule? rule))
        {
            return rule;
        }
        throw new ArgumentException($"Could not find a rule with the nonterminal '{nonterminal}'.", nameof(nonterminal));
    }

    /// <summary>
    /// Gets a rule with the given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal corresponding to the rule.</param>
    /// <returns>The rule for the given nonterminal.</returns>
    /// <exception cref="ArgumentException">No rule with the given nonterminal was found.</exception>
    public Rule GetRule(Nonterminal nonterminal)
    {
        if (_rulesByNonterminal.TryGetValue(nonterminal, out Rule? rule))
        {
            return rule;
        }
        throw new ArgumentException($"Could not find a rule with the nonterminal '{nonterminal}'.", nameof(nonterminal));
    }

    /// <summary>
    /// Try to get a rule with a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal matching the rule.</param>
    /// <param name="rule">The rule with the given nonterminal if it was found. <see langword="null"/> if it was not found.</param>
    /// <returns><see langword="true"/> if a nonterminal was found. <see langword="false"/> if it was not found.</returns>
    public bool TryGetRule(string nonterminal, out Rule? rule)
    {
        if (_nonterminals.TryGetValue(nonterminal, out Nonterminal? nt) && _rulesByNonterminal.TryGetValue(nt!, out rule))
        {
            return true;
        }
        rule = null;
        return false;
    }

    /// <summary>
    /// Try to get a rule with a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal matching the rule.</param>
    /// <param name="rule">The rule with the given nonterminal if it was found. <see langword="null"/> if it was not found.</param>
    /// <returns><see langword="true"/> if a nonterminal was found. <see langword="false"/> if it was not found.</returns>
    public bool TryGetRule(Nonterminal nonterminal, out Rule? rule)
        => _rulesByNonterminal.TryGetValue(nonterminal, out rule);

    /// <summary>
    /// Checks whether a rule with a specific Nonterminal already exists in the grammar.
    /// </summary>
    /// <param name="nonterminal">The nonterminal matching the rule.</param>
    /// <returns><see langword="true"/> if a rule with this nonterminal already exists; <see langword="false"/> otherwise/</returns>
    private bool RuleExists(Nonterminal nonterminal)
        => _rulesByNonterminal.ContainsKey(nonterminal);

    /// <summary>
    /// Checks whether a rule with a specific Nonterminal already exists in the grammar.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal matching the rule.</param>
    /// <returns><see langword="true"/> if a rule with this nonterminal already exists; <see langword="false"/> otherwise/</returns>
    private bool RuleExists(string nonterminal)
        => _nonterminals.TryGetValue(nonterminal, out Nonterminal? nt) && RuleExists(nt!);

    /// <summary>
    /// Checks whether a rule already exists in the grammar.
    /// </summary>
    /// <param name="rule">The rule to check the nonterminal of.</param>
    /// <returns><see langword="true"/> if a rule with this rule's nonterminal already exists; <see langword="false"/> otherwise/</returns>
    private bool RuleExists(Rule rule)
        => RuleExists(rule.Nonterminal);

    /// <summary>
    /// Adds an existing <see cref="Rule"/> to the list of rules in this grammar.
    /// </summary>
    /// <param name="rule">The <see cref="Rule"/> to add to the grammar.</param>
    /// <exception cref="ArgumentException">A rule matching this rule's nonterminal already exists.</exception>
    public void AddRule(Rule rule)
    {
        if (RuleExists(rule))
        {
            throw new ArgumentException($"A rule with the nonterminal '{rule.Nonterminal}' already exists.", nameof(rule));
        }
        rule.Nonterminal = AddNonterminal(rule.Nonterminal.Value);
        _rulesByNonterminal.Add(rule.Nonterminal, rule);
        _rules.Add(rule);
    }

    /// <summary>
    /// Adds a new <see cref="Rule"/> to the list of rules in this grammar.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal associated with the rule.</param>
    /// <returns>The newly added <see cref="Rule"/>.</returns>
    /// <exception cref="ArgumentException">A rule matching this nonterminal already exists.</exception>
    public Rule AddRule(string nonterminal)
    {
        if (RuleExists(nonterminal))
        {
            throw new ArgumentException($"A rule with the nonterminal '{nonterminal}' already exists.", nameof(nonterminal));
        }
        Rule rule = new(AddNonterminal(nonterminal));
        _rulesByNonterminal.Add(rule.Nonterminal, rule);
        _rules.Add(rule);
        return rule;
    }

    /// <summary>
    /// Inserts an existing <see cref="Rule"/> into the list of rules in this grammar.
    /// </summary>
    /// <param name="index">The index to insert the <see cref="Rule"/> at.</param>
    /// <param name="rule">The <see cref="Rule"/> to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the bounds of the list of rules.</exception>
    /// <exception cref="ArgumentException">A rule matching this rule's nonterminal already exists.</exception>
    public void InsertRule(int index, Rule rule)
    {
        if (index < 0 || index >= _rules.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        if (RuleExists(rule))
        {
            throw new ArgumentException($"A rule with the nonterminal '{rule.Nonterminal}' already exists.", nameof(rule));
        }
        _rulesByNonterminal.Add(rule.Nonterminal, rule);
        _rules.Insert(index, rule);
    }

    /// <summary>
    /// Inserts a new <see cref="Rule"/> into the list of rules in this grammar at the specified index.
    /// </summary>
    /// <param name="index">The index to insert the rule at.</param>
    /// <param name="nonterminal">The value of the nonterminal associated with the rule.</param>
    /// <returns>The newly added <see cref="Rule"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the bounds of the list of rules.</exception>
    /// <exception cref="ArgumentException">A rule matching this nonterminal already exists.</exception>
    public Rule InsertRule(int index, string nonterminal)
    {
        if (index < 0 || index >= _rules.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        if (RuleExists(nonterminal))
        {
            throw new ArgumentException($"A rule with the nonterminal '{nonterminal}' already exists.", nameof(nonterminal));
        }
        Rule rule = new(AddNonterminal(nonterminal));
        _rulesByNonterminal.Add(rule.Nonterminal, rule);
        _rules.Insert(index, rule);
        return rule;
    }

    /// <summary>
    /// Removes a rule with the given index, as well as its nonterminal.
    /// </summary>
    /// <param name="index">The index of the rule to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the bounds of the rule list.</exception>
    public void RemoveRuleAt(int index)
    {
        if (index < 0 || index >= _rules.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Rule rule = _rules[index];
        _nonterminals.Remove(rule.Nonterminal.Value);
        _rulesByNonterminal.Remove(rule.Nonterminal);
        _rules.RemoveAt(index);
    }
    #endregion Rules

    #region Symbols

    /// <summary>
    /// Gets a nonterminal in the grammar if one with the given value exists.
    /// </summary>
    /// <param name="value">The value of nonterminal.</param>
    /// <returns>The nonterminal with the given value.</returns>
    /// <exception cref="ArgumentException">No nonterminal with the given value exists in the grammar.</exception>
    public Nonterminal GetNonterminal(string value)
    {
        if (!_nonterminals.TryGetValue(value, out Nonterminal? nonterminal))
        {
            throw new ArgumentException($"Could not find a terminal with the value '{value}'", nameof(value));
        }
        return nonterminal;
    }

    /// <summary>
    /// Adds nonterminal to the grammar.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <returns>The added nonterminal or an existing nonterminal with the same value.</returns>
    public Nonterminal AddNonterminal(string value)
    {
        if (!_nonterminals.TryGetValue(value, out Nonterminal? nonterminal))
        {
            nonterminal = new(value);
            _nonterminals[value] = nonterminal;
        }
        return nonterminal;
    }

    /// <summary>
    /// Gets a terminal in the grammar if one with the given value exists.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <returns>The terminal with the given value.</returns>
    /// <exception cref="ArgumentException">No terminal with the given value exists in the grammar.</exception>
    public Terminal GetTerminal(string value)
    {
        if (!_terminals.TryGetValue(value, out Terminal? terminal))
        {
            throw new ArgumentException($"Could not find a terminal '{value}'", nameof(value));
        }
        return terminal;
    }

    /// <summary>
    /// Adds a terminal to the grammar.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <returns>Either a preexisting terminal with the given value, or a newly created one.</returns>
    public Terminal AddTerminal(string value)
    {
        if (!_terminals.TryGetValue(value, out Terminal? terminal))
        {
            terminal = new(value);
            _terminals[value] = terminal;
        }
        return terminal;
    }

    /// <summary>
    /// Remove a symbol from the grammar.
    /// </summary>
    /// <param name="symbol">The symbol to remove.</param>
    private void RemoveSymbol(ISymbol symbol)
    {
        if (symbol is Nonterminal)
        {
            _ = _nonterminals.Remove(symbol.Value);
        }
        else if (symbol is Terminal)
        {
            _ = _terminals.Remove(symbol.Value);
        }
    }

    /// <summary>
    /// Add a symbol to the grammar.
    /// </summary>
    /// <param name="symbol"></param>
    private void AddSymbol(ISymbol symbol)
    {
        if (symbol is Nonterminal nt)
        {
            _ = _nonterminals.TryAdd(nt.Value, nt);
        }
        else if (symbol is Terminal t)
        {
            _ = _terminals.TryAdd(t.Value, t);
        }
    }

    /// <summary>
    /// Replace a symbol in the grammar with another.
    /// </summary>
    /// <param name="symbol">The symbol to replace.</param>
    /// <param name="newSymbol">The symbol to replace the original with.</param>
    /// <param name="removesymbol"><see langword="true"/> to remove the original symbol from the grammar entirely; <see langword="false"/> otherwise.</param>
    public void ReplaceSymbol(ISymbol symbol, ISymbol newSymbol, bool removesymbol)
    {
        // If I cared a lot about performance, I would have made specific implementations for each combination of symbols; I do not.
        if (removesymbol)
        {
            RemoveSymbol(symbol);
        }

        AddSymbol(newSymbol);

        foreach (Rule rule in _rules)
        {
            rule.ReplaceSymbol(symbol, newSymbol);
        }
    }
    #endregion Symbols

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_rules.Count == 0)
        {
            return "Empty grammar";
        }

        return new StringBuilder().AppendJoin(Environment.NewLine, _rules).ToString();
    }
}
