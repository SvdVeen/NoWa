using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a context-free grammar.
/// </summary>
public class CFG
{
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
    /// <param name="terminal">The terminal to add.</param>
    /// <returns><see langword="true"/> if the <paramref name="terminal"/> did not exist in the grammar yet, otherwise <see langword="false"/>.</returns>
    public bool AddTerminal(Terminal terminal)
    {
        if (_terminalsSet.Add(terminal))
        {
            _terminalsList.Add(terminal);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates a new terminal and adds it to the grammar.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <returns>Either a preexisting terminal with the given <paramref name="value"/>, or a newly created one.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> was empty or whitespace.</exception>
    public Terminal AddTerminal(string value)
    {
        Terminal terminal = Terminal.Get(value);
        _ = AddTerminal(terminal);
        return terminal;
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

    /// <summary>
    /// Removes the terminal at the given index.
    /// </summary>
    /// <param name="index">The index of the terminal to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside of the bounds of the terminal list.</exception>
    public void RemoveTerminalAt(int index)
    {
        if (index < 0 || index >= _terminalsList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Terminal terminal = _terminalsList[index];
        _terminalsList.RemoveAt(index);
        if (!_terminalsSet.Remove(terminal))
        {
            throw new InvalidOperationException("Terminal could not be removed from set."); // This should never occur!
        }
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
    /// Adds a nonterminal to the grammar.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add.</param>
    /// <returns><see langword="true"/> if the <paramref name="nonterminal"/> did not exist in the grammar yet, otherwise <see langword="false"/>.</returns>
    public bool AddNonterminal(Nonterminal nonterminal)
    {
        if (_nonterminalsSet.Add(nonterminal))
        {
            _nonterminalsList.Add(nonterminal);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds nonterminal to the grammar.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <returns>Either a preexisting nonterminal with the given <paramref name="value"/>, or a newly created one.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> was empty or whitespace.</exception>
    public Nonterminal AddNonterminal(string value)
    {
        Nonterminal nonterminal = Nonterminal.Get(value);
        _ = AddNonterminal(nonterminal);
        return nonterminal;
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
        if (!_nonterminalsSet.Remove(nonterminal))
        {
            throw new InvalidOperationException("Nonterminal could not be removed from set."); // This should never occur!
        }
    }
    #endregion Nonterminals

    #region Rules
    protected readonly Dictionary<Nonterminal, Rule> _rulesByNonterminal = new();
    protected readonly List<Rule> _rules = new();

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

    #region Productions
    protected readonly List<Production> _productions = new();
    protected readonly Dictionary<Nonterminal, List<Production>> _productionsByHead = new();

    /// <summary>
    /// Gets the list of productions in the grammar.
    /// </summary>
    public IReadOnlyList<Production> Productions { get => _productions; }

    /// <summary>
    /// Adds a production to the grammar.
    /// Also automatically updates the terminals and nonterminals in the grammar.
    /// </summary>
    /// <param name="production">The production to add.</param>
    public void AddProduction(Production production)
    {
        _productions.Add(production);
        if (!_productionsByHead.ContainsKey(production.Head))
        {
            _productionsByHead.Add(production.Head, new());
        }
        _productionsByHead[production.Head].Add(production);
        AddSymbols(production);
    }

    /// <summary>
    /// Adds all symbols in a production to the grammar.
    /// </summary>
    /// <param name="production">The production to add symbols from.</param>
    private void AddSymbols(Production production)
    {
        _ = AddNonterminal(production.Head);
        foreach(ISymbol symbol in production.Body)
        {
            if (symbol is Terminal terminal)
            {
                _ = AddTerminal(terminal);
            }
            else if (symbol is Nonterminal nonterminal)
            {
                _ = AddNonterminal(nonterminal);
            }
        }
    }

    /// <summary>
    /// Gets the list of productions with the given head.
    /// </summary>
    /// <param name="head">The nonterminal to find all productions for.</param>
    /// <returns>A list of all productions that have the given <paramref name="head"/>.</returns>
    public IReadOnlyList<Production> GetProductionsByHead(Nonterminal head)
    {
        if (!_productionsByHead.TryGetValue(head, out List<Production>? productions))
        {
            productions = new(0);
        }
        return productions!;
    }

    /// <summary>
    /// Removes a production from the grammar. Does not remove its symbols.
    /// </summary>
    /// <param name="production">The production to remove.</param>
    /// <returns><see langword="true"/> if the <paramref name="production"/> was successfully removed from the grammar, otherwise <see langword="false"/>.</returns>
    public bool RemoveProduction(Production production)
    {
        if (_productions.Remove(production))
        {
            if (!_productionsByHead.TryGetValue(production.Head, out List<Production>? productions))
            {
                throw new InvalidOperationException($"{nameof(production)} does not have an entry in {nameof(_productionsByHead)}."); // This should never occur!
            }
            if (!productions.Remove(production))
            {
                throw new InvalidOperationException($"{nameof(production)} could not be removed from its entry in {nameof(_productionsByHead)}."); // This should never occur!
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes the production at the given index.
    /// </summary>
    /// <param name="index">The index of the production to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside of the bounds of the production list.</exception>
    public void RemoveProductionAt(int index)
    {
        if (index < 0 || index >= _productions.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Production production = _productions[index];
        _productions.RemoveAt(index);
        if (!_productionsByHead.TryGetValue(production.Head, out List<Production>? productions))
        {
            throw new InvalidOperationException($"{nameof(production)} does not have an entry in {nameof(_productionsByHead)}."); // This should never occur!
        }
        if (!productions.Remove(production))
        {
            throw new InvalidOperationException($"{nameof(production)} could not be removed from its entry in {nameof(_productionsByHead)}."); // This should never occur!
        }
    }
    #endregion Productions

    /// <summary>
    /// Clears the grammar of all its elements.
    /// </summary>
    public void Clear()
    {
        _terminalsSet.Clear();
        _terminalsList.Clear();
        _nonterminalsSet.Clear();
        _nonterminalsList.Clear();
        _productions.Clear();
        _productionsByHead.Clear();
    }

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

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_productions.Count == 0)
        {
            return "Empty grammar";
        }

        return new StringBuilder().AppendJoin(Environment.NewLine, _productionsByHead.Select(ProductionsToString)).ToString();
    }

    private string ProductionsToString(KeyValuePair<Nonterminal, List<Production>> group)
    {
        return new StringBuilder($"{group.Key} = ").AppendJoin(" | ", group.Value.Select(p => p.Body)).Append(" ;").ToString();
    }
}
