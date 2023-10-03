using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="UnreachableSymbolsStep"/>.
/// </summary>
[TestClass]
public class UnreachableSymbolsStepTests
{
    /// <summary>
    /// Tests the <see cref="UnreachableSymbolsStep"/>.
    /// </summary>
    /// <remarks>
    /// This unit test is based on the example given by Hopcroft, Motwani, and Ullman on page 262 of the 2013 edition
    /// of "Introduction to Automata Theory, Languages, and Computation: Pearson New International Edition" 
    /// </remarks>
    [TestMethod]
    public void TestUnreachableSymbols()
    {
        CFG grammar = new();

        // Add rule S = A B | 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("B")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("a")));

        // Add rule A = 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("b")));

        UnreachableSymbolsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual("S = 'a' ;", grammar.ToString());
        Assert.AreEqual(1, grammar.Nonterminals.Count);
        Assert.AreSame(Nonterminal.Get("S"), grammar.Nonterminals[0]);
        Assert.AreEqual(1, grammar.Terminals.Count);
        Assert.AreSame(Terminal.Get("a"), grammar.Terminals[0]);
    }

    /// <summary>
    /// An additional, more complicated test of the <see cref="UnreachableSymbolsStep"/>.
    /// </summary>
    [TestMethod]
    public void TestManyUnreachableSymbols()
    {
        CFG grammar = new();

        // Add rule S = A B C | A | 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("B"), Nonterminal.Get("C")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("a")));

        // Add rule A = C | 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Nonterminal.Get("C")));
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        // Add rule C = 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

        // Add rule D = 'd' ;
        grammar.AddProduction(new(Nonterminal.Get("D"), Terminal.Get("d")));

        UnreachableSymbolsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A | 'a' ;{Environment.NewLine}" +
            $"A = C | 'a' ;{Environment.NewLine}" +
            $"C = 'c' ;", grammar.ToString());
        Assert.AreEqual(5, grammar.Productions.Count);
        Assert.AreEqual(3, grammar.Nonterminals.Count);
        Assert.AreEqual(2, grammar.Terminals.Count);
    }
}