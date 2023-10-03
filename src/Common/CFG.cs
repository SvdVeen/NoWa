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
        AddProductionByHead(production);
        AddSymbols(production);
    }

    /// <summary>
    /// Adds a production to the <see cref="_productionsByHead"/> dictionary.
    /// </summary>
    /// <param name="production">The production to add.</param>
    private void AddProductionByHead(Production production)
    {
        if (!_productionsByHead.ContainsKey(production.Head))
        {
            _productionsByHead.Add(production.Head, new());
        }
        _productionsByHead[production.Head].Add(production);
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
            RemoveProductionByHead(production);
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
        RemoveProductionByHead(production);
    }

    /// <summary>
    /// Removes a production from the <see cref="_productionsByHead"/> dictionary.
    /// Automatically clears a nonterminal's entry if no productions remain.
    /// </summary>
    /// <param name="production">The production to remove.</param>
    /// <exception cref="InvalidOperationException">Something went wrong while removing the production. This really should not occur unless threading shenanigans are at play.</exception>
    /// <remarks>This must be called for a production that is actually part of the grammar!</remarks>
    private void RemoveProductionByHead(Production production)
    {
        if (!_productionsByHead.TryGetValue(production.Head, out List<Production>? productions))
        {
            throw new InvalidOperationException($"{nameof(production)} does not have an entry in {nameof(_productionsByHead)}."); // This should never occur!
        }
        if (!productions.Remove(production))
        {
            throw new InvalidOperationException($"{nameof(production)} could not be removed from its entry in {nameof(_productionsByHead)}."); // This should never occur!
        }
        if (productions.Count == 0 && !_productionsByHead.Remove(production.Head))
        {
            throw new InvalidOperationException($"The entry for {production.Head} could not be removed from {nameof(_productionsByHead)}."); // This should never occur!
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

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_productions.Count == 0)
        {
            return "Empty grammar";
        }

        return new StringBuilder().AppendJoin(Environment.NewLine, _productionsByHead.Select(ProductionsToString)).ToString();
    }

    /// <summary>
    /// Shorthand for quickly printing entries in <see cref="_productionsByHead"/>.
    /// </summary>
    /// <param name="group">A pair of a nonterminal and all productions with it as their heads.</param>
    /// <returns>A formatted string for displaying all productions of a nonterminal.</returns>
    private string ProductionsToString(KeyValuePair<Nonterminal, List<Production>> group)
    {
        return new StringBuilder($"{group.Key} = ").AppendJoin(" | ", group.Value.Select(p => p.Body)).Append(" ;").ToString();
    }
}
