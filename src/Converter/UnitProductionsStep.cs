using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates all unit productions in a grammar.
/// </summary>
public sealed class UnitProductionsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public UnitProductionsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminates all unit productions in the given <see cref="WAG"/>.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        Logger.LogInfo("Eliminating unit productions...");
        GrammarStats stats = new(grammar);

        var unitPairs = GetUnitPairs(grammar);

        Grammar originalGrammar = grammar.Clone();
        var lookup = originalGrammar.Productions.ToLookup(p => p.Head);

        Nonterminal? StartSymbol = grammar.StartSymbol;
        grammar.Clear();
        grammar.StartSymbol = StartSymbol;

        foreach (var pair in unitPairs.OrderBy(p => originalGrammar.Nonterminals.ToList().IndexOf(p.Item1)))
        {
            foreach(Production production in lookup[pair.Item2])
            {
                if (production.Body.Count > 1 || production.Body.Count == 1 && production.Body[0] is not Nonterminal)
                {
                    grammar.AddProduction(new(pair.Item1, production.Weight, production.Body, production.Expressions));
                    if (!pair.Item1.Equals(production.Head))
                    {
                        // Transfer the attributes of the original production's head to the new one's head.
                        if (grammar is WAG wag)
                        {
                            foreach (char attr in ((WAG)originalGrammar).GetInheritedAttributes(production.Head))
                            {
                                wag.AddInheritedAttribute(pair.Item1, attr);
                            }
                            foreach (char attr in ((WAG)originalGrammar).GetSynthesizedAttributes(production.Head))
                            {
                                wag.AddSynthesizedAttribute(pair.Item1, attr);
                            }
                            foreach (char attr in ((WAG)originalGrammar).GetStaticAttributes(production.Head))
                            {
                                wag.AddStaticAttribute(pair.Item1, attr);
                            }
                        }
                    }
                }
            }
        }

        Logger.LogInfo("Elminated unit productions.");
        stats.LogDiff(grammar, Logger);
    }

    /// <summary>
    /// Gets all unit pairs in a grammar.
    /// </summary>
    /// <param name="grammar">The grammar to get unit pairs from.</param>
    /// <returns>A set of all unit pairs in the grammar.</returns>
    private ISet<Tuple<Nonterminal,Nonterminal>> GetUnitPairs(Grammar grammar)
    {
        HashSet<Tuple<Nonterminal,Nonterminal>> pairs = new();
        // Base step: every nonterminal pairs with itself.
        foreach (Nonterminal nonterminal in grammar.Nonterminals)
        {
            if (pairs.Add(new(nonterminal, nonterminal)))
            {
                Logger.LogDebug($"Adding unit pair ({nonterminal}, {nonterminal})");
            }
        }

        // Induction step: if there exists a unit pair (A,B), and a production B -> C, where C is a variable, then (A,C) is a unit pair.
        // Repeat this until no more new pairs are found.
        HashSet<Tuple<Nonterminal, Nonterminal>> oldPairs;
        do
        {
            oldPairs = new(pairs);
            foreach (var pair in oldPairs)
            {
                foreach (Production production in grammar.GetProductionsByHead(pair.Item2))
                {
                    if (production.Body.Count == 1 && production.Body[0] is Nonterminal nonterminal)
                    {
                        Tuple<Nonterminal, Nonterminal> newPair = new(pair.Item1, nonterminal);
                        if (pairs.Add(newPair))
                        {
                            Logger.LogDebug($"Adding unit pair {newPair}");
                        }
                    }
                }
            }
        } while (oldPairs.Count < pairs.Count);

        return pairs;
    }
}
