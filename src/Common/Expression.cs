using System.Collections;
using System.Text;

namespace NoWa.Common;

/// <summary>
/// Represents a combination of symbols.
/// </summary>
public class Expression : IList<ISymbol>
{
    private readonly IList<ISymbol> _symbols;

    /// <inheritdoc/>
    public ISymbol this[int index]
    {
        get => _symbols[index];
        set => _symbols[index] = value;
    }

    /// <inheritdoc/>
    public int Count => _symbols.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => _symbols.IsReadOnly;

    /// <summary>
    /// Creates a new instance of the <see cref="Expression"/> class.
    /// </summary>
    public Expression()
    {
        _symbols = new List<ISymbol>();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Expression"/> class.
    /// </summary>
    /// <param name="symbols">The symbols in this expression.</param>
    public Expression(IEnumerable<ISymbol> symbols)
    {
        _symbols = new List<ISymbol>(symbols);
    }

    /// <inheritdoc/>
    public void Add(ISymbol item)
    {
        _symbols.Add(item);
    }
    /// <inheritdoc/>
    public void Clear()
    {
        _symbols.Clear();
    }
    /// <inheritdoc/>
    public bool Contains(ISymbol item)
    {
        return _symbols.Contains(item);
    }
    /// <inheritdoc/>
    public void CopyTo(ISymbol[] array, int arrayIndex)
    {
        _symbols.CopyTo(array, arrayIndex);
    }
    /// <inheritdoc/>
    public IEnumerator<ISymbol> GetEnumerator()
    {
        return _symbols.GetEnumerator();
    }
    /// <inheritdoc/>
    public int IndexOf(ISymbol item)
    {
        return _symbols.IndexOf(item);
    }
    /// <inheritdoc/>
    public void Insert(int index, ISymbol item)
    {
        _symbols.Insert(index, item);
    }
    /// <inheritdoc/>
    public bool Remove(ISymbol item)
    {
        return _symbols.Remove(item);
    }
    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        _symbols.RemoveAt(index);
    }

    /// <summary>
    /// Replace all occurrences of a symbol in the expression with another.
    /// </summary>
    /// <param name="symbol">The symbol to replace.</param>
    /// <param name="newSymbol">The symbol to replace the original with.</param>
    public void Replace(ISymbol symbol, ISymbol newSymbol)
    {
        for (int i = 0; i < _symbols.Count; i++)
        {
            if (_symbols[i].Equals(symbol))
            {
                _symbols[i] = newSymbol;
            }
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_symbols).GetEnumerator();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_symbols.Count == 0)
        {
            return "Empty expression";
        }

        return new StringBuilder().AppendJoin(' ', _symbols).ToString();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Expression expression && _symbols.SequenceEqual(expression._symbols);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int result = 17;
        foreach (var symbol in _symbols)
        {
            result = result * 19 + symbol.GetHashCode();
        }
        return result;
    }
}
