namespace NoWa.Common;

/// <summary>
/// A container class for a reference to a symbol. Allows for fast replacing of symbols.
/// </summary>
internal class SymbolReference
{
    private ISymbol _symbol;


    /// <summary>
    /// Gets the symbol as a <see cref="Common.Nonterminal"/>
    /// </summary>
    /// <exception cref="InvalidCastException">The referenced symbol is not a nonterminal.</exception>
    public Nonterminal Nonterminal
    {
        get
        {
            if (_symbol is not Nonterminal nonterminal)
                throw new InvalidCastException($"Cannot get the symbol \"{_symbol.Value}\" as a nonterminal.");
            return nonterminal;
        }
    }

    /// <summary>
    /// Gets or sets the symbol referenced by this instance.
    /// </summary>
    public ISymbol Symbol
    {
        get
        {
            return _symbol;
        }
        set
        {
            _symbol = value;
        }
    }

    /// <summary>
    /// Gets the symbol as a <see cref="Common.Terminal"/>.
    /// </summary>
    public Terminal Terminal
    {
        get
        {
            if (_symbol is not Terminal terminal)
                throw new InvalidCastException($"Cannot get the symbol \"{_symbol.Value}\" as a terminal.");
            return terminal;
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SymbolReference"/> class.
    /// </summary>
    /// <param name="symbol">The symbol to reference.</param>
    internal SymbolReference(ISymbol symbol)
    {
        _symbol = symbol;
    }
}
