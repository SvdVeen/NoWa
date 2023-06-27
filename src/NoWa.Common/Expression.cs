using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a combination of symbols.
/// </summary>
public class Expression
{
    private readonly Grammar _grammar;
    private readonly IList<int> _symbols;

    /// <summary>
    /// Access symbols in the expression by their index.
    /// </summary>
    /// <param name="i">The index of a symbol.</param>
    /// <returns>The symbol at the given index.</returns>
    public ISymbol this[int i]
    {
        get => GetSymbol(i);
    }

    /// <summary>
    /// Gets the number of symbols in the expression.
    /// </summary>
    public int Count
    { 
        get => _symbols.Count; 
    }

    /// <summary>
    /// Gets the enumerator over the symbols in this expression.
    /// </summary>
    public IEnumerable<ISymbol> Symbols
    {
        get
        {
            for (int i = 0; i < _symbols.Count; i++)
                yield return _grammar.GetSymbol(i);
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Expression"/> class.
    /// </summary>
    /// <param name="grammar">The <see cref="Grammar"/> this <see cref="Expression"/> belongs to.</param>
    internal Expression(Grammar grammar)
    {
        _grammar = grammar;
        _symbols = new List<int>();
    }


    /// <summary>
    /// Creates a new instance of the <see cref="Expression"/> class.
    /// </summary>
    /// <param name="grammar">The <see cref="Grammar"/> this <see cref="Expression"/> belongs to.</param>
    /// <param name="symbols">The indices of the symbols in this <see cref="Expression"/>.</param>
    internal Expression(Grammar grammar, params int[] symbols)
    {
        _grammar = grammar;
        _symbols = new List<int>(symbols);
    }

    /// <summary>
    /// Adds a nonterminal to the expression.
    /// </summary>
    /// <param name="value">The value of the nonterminal to add.</param>
    public void AddNonterminal(string value)
    {
        Nonterminal nonterminal = _grammar.GetOrCreateNonterminal(value);
        _symbols.Add(_grammar.GetSymbolIndex(nonterminal));
    }

    /// <summary>
    /// Adds a terminal to the expression.
    /// </summary>
    /// <param name="value">The value of the terminal to add.</param>
    public void AddTerminal(string value)
    {
        Terminal terminal = _grammar.GetOrCreateTerminal(value);
        _symbols.Add(_grammar.GetSymbolIndex(terminal));
    }

    /// <summary>
    /// Gets a symbol by its index.
    /// </summary>
    /// <param name="index">The index of the symbol to get.</param>
    /// <returns>The symbol at the given index.</returns>
    public ISymbol GetSymbol(int index) => _grammar.GetSymbol(index);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">The expression contains no symbols.</exception>
    public override string ToString()
    {
        if (_symbols.Count == 0)
            throw new InvalidOperationException("Expression contains no symbols.");

        StringBuilder sb = new();
        _ = sb.Append($"{GetSymbol(0)}");
        for (int i = 1; i < _symbols.Count; i++)
            _ = sb.Append($" {GetSymbol(i)}");
        return sb.ToString();
    }
}
