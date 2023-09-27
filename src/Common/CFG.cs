﻿using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free grammar.
/// </summary>
public class CFG
{
    protected readonly Dictionary<Nonterminal, Rule> _rulesByNonterminal = new();
    protected readonly List<Rule> _rules = new();

    #region Symbols

    #region Terminals
    protected readonly List<Terminal> _terminalsList = new();
    protected readonly HashSet<Terminal> _terminalsSet = new();


    /// <summary>
    /// Gets the list of all terminals in the grammar.
    /// </summary>
    public IReadOnlyList<Terminal> Terminals { get => _terminalsList; }

    /// <summary>
    /// Adds a terminal to the grammar.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <returns>Either a preexisting terminal with the given value, or a newly created one.</returns>
    public Terminal AddTerminal(string value)
    {
        Terminal terminal = Terminal.Get(value);
        if (_terminalsSet.Add(terminal))
        {
            _terminalsList.Add(terminal);
        }
        return terminal;
    }

    /// <summary>
    /// Removes the terminal at the given index.
    /// </summary>
    /// <param name="index">The index of the terminal to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside of the bounds of the nonterminal list.</exception>
    public void RemoveTerminalAt(int index)
    {
        if (index < 0 || index >= _terminalsList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Terminal terminal = _terminalsList[index];
        _terminalsList.RemoveAt(index);
        _terminalsSet.Remove(terminal);
    }

    /// <summary>
    /// Checks whether the grammar contains a terminal.
    /// </summary>
    /// <param name="terminal">The terminal to check for.</param>
    /// <returns><see langword="true"/> if the grammar contains the given <paramref name="terminal"/>, <see langword="false"/> otherwise.</returns>
    public bool ContainsTerminal(Terminal terminal)
    {
        return _terminalsSet.Contains(terminal);
    }

    #endregion Terminals

    #region Nonterminals
    protected readonly List<Nonterminal> _nonterminalsList = new();
    protected readonly HashSet<Nonterminal> _nonterminalsSet = new();

    /// <summary>
    /// Gets the list of all nonterminals in the grammar.
    /// </summary>
    public IReadOnlyList<Nonterminal> Nonterminals { get => _nonterminalsList; }

    /// <summary>
    /// Adds nonterminal to the grammar.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <returns>The added nonterminal or an existing nonterminal with the same value.</returns>
    public Nonterminal AddNonterminal(string value)
    {
        Nonterminal nonterminal = Nonterminal.Get(value);
        if (_nonterminalsSet.Add(nonterminal))
        {
            _nonterminalsList.Add(nonterminal);
        }
        return nonterminal;
    }

    /// <summary>
    /// Remove the nonterminal at the given index.
    /// </summary>
    /// <param name="index">The index of the nonterminal to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside of the bounds of the nonterminal list.</exception>
    public void RemoveNonterminalAt(int index)
    {
        if (index < 0 || index >= _nonterminalsList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Nonterminal nonterminal = _nonterminalsList[index];
        _nonterminalsList.RemoveAt(index);
        _nonterminalsSet.Remove(nonterminal);
    }

    /// <summary>
    /// Checks whether the grammar contains a nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to check for.</param>
    /// <returns><see langword="true"/> if the grammar contains the given <paramref name="nonterminal"/>, <see langword="false"/> otherwise.</returns>
    public bool ContainsNonterminal(Nonterminal nonterminal)
    {
        return _nonterminalsSet.Contains(nonterminal);
    }
    #endregion Nonterminals

    /// <summary>
    /// Remove a symbol from the grammar.
    /// </summary>
    /// <param name="symbol">The symbol to remove.</param>
    private void RemoveSymbol(ISymbol symbol)
    {
        if (symbol is Nonterminal nonterminal)
        {
            if (_nonterminalsSet.Remove(nonterminal))
            {
                _ = _nonterminalsList.Remove(nonterminal);
            }
        }
        else if (symbol is Terminal terminal)
        {
            if (_terminalsSet.Remove(terminal))
            {
                _ = _terminalsList.Remove(terminal);
            }
        }
    }

    /// <summary>
    /// Add a symbol to the grammar.
    /// </summary>
    /// <param name="symbol">The symbol to add.</param>
    private void AddSymbol(ISymbol symbol)
    {
        if (symbol is Nonterminal nonterminal)
        {
            if (_nonterminalsSet.Add(nonterminal))
            {
                _nonterminalsList.Add(nonterminal);
            }
        }
        else if (symbol is Terminal terminal)
        {
            if (_terminalsSet.Add(terminal))
            {
                _terminalsList.Add(terminal);
            }
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

    #region Rules

    /// <summary>
    /// Gets a list of rules in this grammar.
    /// </summary>
    public IReadOnlyList<Rule> Rules { get => _rules; }

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
        Nonterminal nt = Nonterminal.Get(nonterminal);
        if (_nonterminalsSet.Contains(nt) && _rulesByNonterminal.TryGetValue(nt, out Rule? rule))
        {
            return rule!;
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
            return rule!;
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
        Nonterminal nt = Nonterminal.Get(nonterminal);
        if (_nonterminalsSet.Contains(nt) && _rulesByNonterminal.TryGetValue(nt, out rule))
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
    {
        Nonterminal nt = Nonterminal.Get(nonterminal);
        return _nonterminalsSet.Contains(nt) && RuleExists(nt);
    }

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
        _nonterminalsSet.Remove(rule.Nonterminal);
        _nonterminalsList.Remove(rule.Nonterminal);
        _rulesByNonterminal.Remove(rule.Nonterminal);
        _rules.RemoveAt(index);
    }
    #endregion Rules

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
