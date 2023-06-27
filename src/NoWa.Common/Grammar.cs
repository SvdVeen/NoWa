using System.Data;
using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a grammar.
/// </summary>
public class Grammar
{
    private readonly List<ISymbol> _symbols = new();
    private readonly Dictionary<string, int> _nonterminals = new();
    private readonly Dictionary<string, int> _terminals = new();
    private readonly Dictionary<Nonterminal, Rule> _rulesByNonterminal = new();
    private readonly List<Rule> _rules = new();

    /// <summary>
    /// Gets the number of rules in the grammar.
    /// </summary>
    public int RuleCount { get => _rules.Count; }

    /// <summary>
    /// Gets or creates a nonterminal with the given value.
    /// </summary>
    /// <param name="value">The value of the nonterminal.</param>
    /// <returns>Either a preexisting nonterminal with the given value, or a newly created one.</returns>
    /// <exception cref="InvalidOperationException">The stored symbol at the found index is not of the right type.</exception>
    public Nonterminal GetOrCreateNonterminal(string value)
    {
        if (!_nonterminals.TryGetValue(value, out int index))
        {
            Nonterminal newNonterminal = new(value);
            _symbols.Add(newNonterminal);
            _nonterminals[value] = _symbols.Count - 1;
            return newNonterminal;
        }
        if (_symbols[index] is not Nonterminal nonterminal)
            throw new InvalidOperationException($"Expected symbol \"{value}\" to be a nonterminal when it was not.");
        return nonterminal;
    }

    /// <summary>
    /// Gets or creates a terminal with the given value.
    /// </summary>
    /// <param name="value">The value of the terminal.</param>
    /// <returns>Either a preexisting terminal with the given value, or a newly created one.</returns>
    /// <exception cref="InvalidOperationException">The stored symbol at the found index is not of the right type.</exception>
    public Terminal GetOrCreateTerminal(string value)
    {
        if (!_terminals.TryGetValue(value, out int index))
        {
            Terminal newTerminal = new(value);
            _symbols.Add(newTerminal);
            _terminals[value] = _symbols.Count - 1;
            return newTerminal;
        }
        if (_symbols[index] is not Terminal terminal)
            throw new InvalidOperationException($"Expected symbol \"{value}\" to be a terminal when it was not.");
        return terminal;
    }

    /// <summary>
    /// Creates a new expression without symbols.
    /// </summary>
    /// <returns>The newly created expression.</returns>
    public Expression CreateExpression()
    {
        return new Expression(this);
    }

    public Expression CreateExpression(params ISymbol[] symbols)
    {
        return new Expression(this, symbols.Select(GetSymbolIndex).ToArray());
    }

    /// <summary>
    /// Create a new rule in the grammar.
    /// </summary>
    /// <param name="nonterminal">The value of the nonterminal corresponding to the rule.</param>
    /// <param name="exprs">The expressions the nonterminal translates to.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">There is already a rule associated with the given nonterminal.</exception>
    public Rule CreateRule(string nonterminal, params Expression[] exprs)
    {
        Nonterminal nt = GetOrCreateNonterminal(nonterminal);
        Rule rule = new(this, _nonterminals[nonterminal], new List<Expression>(exprs));
        if (!_rulesByNonterminal.TryAdd(nt, rule))
            throw new InvalidOperationException($"Could not add rule {nonterminal} because there is already a rule associated with it.");
        _rules.Add(rule);
        return rule;
    }

    /// <summary>
    /// Gets a symbol in the grammar by its index.
    /// </summary>
    /// <param name="index">The index of the symbol.</param>
    /// <returns>The symbol at the given index.</returns>
    internal ISymbol GetSymbol(int index)
    {
        return _symbols[index];
    }

    /// <summary>
    /// Gets the index of a symbol in the grammar.
    /// </summary>
    /// <param name="symbol">The symbol to get the index of.</param>
    /// <returns>The index of the given symbol</returns>
    internal int GetSymbolIndex(ISymbol symbol)
    { 
        // This is potentially slow.
        return _symbols.IndexOf(symbol);
    }

    /// <summary>
    /// Gets a rule with the given index.
    /// </summary>
    /// <param name="index">The index of the rule to get.</param>
    /// <returns>The rule with the given index.</returns>
    public Rule GetRule(int index) => _rules[index];

    public void ReplaceSymbol(ISymbol original, ISymbol replacement)
    {
        _symbols[GetSymbolIndex(original)] = replacement;
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
