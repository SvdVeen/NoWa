using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a grammar.
/// </summary>
public class Grammar
{
    private readonly Dictionary<string, Nonterminal> _nonterminals = new();
    private readonly Dictionary<string, Terminal> _terminals = new();
    private readonly List<Rule> _rules = new();

    /// <summary>
    /// Gets the number of rules in the grammar.
    /// </summary>
    public int RuleCount { get => _rules.Count; }

    /// <summary>
    /// Adds an existing <see cref="Rule"/> to the list of rules in this grammar.
    /// </summary>
    /// <param name="rule">The <see cref="Rule"/> to add to the grammar.</param>
    public void AddRule(Rule rule) => _rules.Add(rule);

    /// <summary>
    /// Adds a new <see cref="Rule"/> to the list of rules in this grammar.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal associated with the rule.</param>
    /// <returns>The newly added <see cref="Rule"/>.</returns>
    public Rule AddRule(string nonterminal)
    {
        Rule rule = new(GetOrCreateNonterminal(nonterminal));
        AddRule(rule);
        return rule;
    }

    /// <summary>
    /// Inserts an existing <see cref="Rule"/> into the list of rules in this grammar.
    /// </summary>
    /// <param name="index">The index to insert the <see cref="Rule"/> at</param>
    /// <param name="rule">The <see cref="Rule"/> to insert.</param>
    public void InsertRule(int index, Rule rule) => _rules.Insert(index, rule);

    /// <summary>
    /// Inserts a new <see cref="Rule"/> into the list of rules in this grammar at the specified index.
    /// </summary>
    /// <param name="index">The index to insert the rule at.</param>
    /// <param name="nonterminal">The value of the nonterminal associated with the rule.</param>
    /// <returns></returns>
    public Rule InsertRule(int index, string nonterminal)
    {
        Rule rule = new(GetOrCreateNonterminal(nonterminal));
        InsertRule(index, rule);
        return rule;
    }

    /// <summary>
    /// Gets or creates a nonterminal with the given value.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <returns>Either a preexisting nonterminal with the given value, or a newly created one.</returns>
    public Nonterminal GetOrCreateNonterminal(string value)
    {
        if (!_nonterminals.TryGetValue(value, out Nonterminal? nonterminal))
        {
            nonterminal = new(value);
            _nonterminals[value] = nonterminal;
        }
        return nonterminal;
    }

    /// <summary>
    /// Gets or creates a terminal with the given value.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <returns>Either a preexisting terminal with the given value, or a newly created one.</returns>
    public Terminal GetOrCreateTerminal(string value)
    {
        if (!_terminals.TryGetValue(value, out Terminal? terminal))
        {
            terminal = new(value);
            _terminals[value] = terminal;
        }
        return terminal;
    }

    /// <summary>
    /// Gets a rule with the given index.
    /// </summary>
    /// <param name="index">The index of the rule to get.</param>
    /// <returns>The rule with the given index.</returns>
    public Rule GetRule(int index) => _rules[index];

    /// <summary>
    /// Replace a symbol in the grammar with another.
    /// </summary>
    /// <param name="original">The original symbol.</param>
    /// <param name="replacement">The symbol that should replace it.</param>
    /// <param name="removeOriginal"><see langword="true"/> to have the original symbol deleted entirely from the grammar, otherwise <see langword="false"/>.</param>
    public void ReplaceSymbol(Terminal original, Nonterminal replacement, bool removeOriginal = true)
    {
        if (removeOriginal)
        {
            _ = _terminals.Remove(original.Value);
        }
        _ = _nonterminals.TryAdd(replacement.Value, replacement);
        foreach (var expr in _rules.SelectMany(rule => rule.Expressions))
        {
            for (int i = 0; i < expr.Count; i++)
            {
                if (expr[i] == original)
                {
                    expr[i] = replacement;
                }
            }
        }
    }

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
