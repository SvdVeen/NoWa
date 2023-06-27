using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a grammar.
/// </summary>
public class Grammar
{
    private readonly Dictionary<string, Nonterminal> _nonterminals = new();
    private readonly Dictionary<Nonterminal, Rule> _rulesByNonterminal = new();
    private readonly List<Rule> _rules = new();

    /// <summary>
    /// Gets the number of nonterminals in the grammar.
    /// </summary>
    public int NonterminalCount { get => _nonterminals.Count; }

    /// <summary>
    /// Gets the number of rules in the grammar.
    /// </summary>
    public int RuleCount { get => _rules.Count; }

    /// <summary>
    /// Adds a nonterminal to the grammar.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to add to the grammar.</param>
    public void AddNonterminal(Nonterminal nonterminal) => _ = _nonterminals.TryAdd(nonterminal.Value, nonterminal);

    /// <summary>
    /// Adds a rule to the grammar if no rule for its nonterminal exists yet.
    /// </summary>
    /// <param name="nonterminal">The nonterinal the rule corresponds to.</param>
    /// <param name="rule">The rule for the given nonterminal.</param>
    /// <exception cref="InvalidOperationException">The given nonterminal already has a rule associated with it.</exception>
    public void AddRule(Nonterminal nonterminal, Rule rule)
    {
        if (!_rulesByNonterminal.TryAdd(nonterminal, rule))
            throw new InvalidOperationException($"Could not add rule {nonterminal} because there is already a rule associated with it.");

        _ = _nonterminals.TryAdd(nonterminal.Value, nonterminal);
        _rules.Add(rule);
    }

    /// <summary>
    /// Get a nonterminal in the grammar.
    /// </summary>
    /// <param name="index">The index of the nonterminal to get.</param>
    /// <returns>The nonterminal with the given index.</returns>
    public Nonterminal GetNonterminal(int index) => _nonterminals.Values.ElementAt(index);

    /// <summary>
    /// Gets a nonterminal in the grammar.
    /// </summary>
    /// <param name="value">The value of the nonterminal to get.</param>
    /// <returns>The nonterminal with the corresponding value.</returns>
    public Nonterminal GetNonterminal(string value) => _nonterminals[value];

    /// <summary>
    /// Gets a rule with the given index.
    /// </summary>
    /// <param name="index">The index of the rule to get.</param>
    /// <returns>The rule with the given index.</returns>
    public Rule GetRule(int index) => _rules[index];

    /// <summary>
    /// Gets the rule associated with a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal to get the rule for.</param>
    /// <returns>The rule associated with the given nonterminal.</returns>
    public Rule GetRule(string nonterminal) => _rulesByNonterminal[_nonterminals[nonterminal]];

    /// <summary>
    /// Gets the rule associated with a given nonterminal.
    /// </summary>
    /// <param name="nonterminal">The nonterminal to get the rule for.</param>
    /// <returns>The rule associated with the given nonterminal.</returns>
    public Rule GetRule(Nonterminal nonterminal) => _rulesByNonterminal[nonterminal];

    /// <summary>
    /// Inserts a rule into the grammar with the given index.
    /// </summary>
    /// <param name="index">The index to insert the rule at.</param>
    /// <param name="rule">The rule to insert.</param>
    /// <returns><see langword="true"/> if the rule could be inserted, otherwise <see langword="false"/></returns>
    public bool InsertRule(int index,  Rule rule)
    {
        if (index >= _rules.Count) return false;
        _rules.Insert(index, rule);
        return true;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_rules.Count == 0)
            return ""; // This is actually not supposed to happen.

        StringBuilder sb = new();
        foreach (Rule rule in _rules)
            _ = sb.AppendLine($"{rule}");
        return sb.ToString();
    }
}
