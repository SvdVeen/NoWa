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
        Grammar grammar = new();

        Terminal a = new("a");
        Terminal b = new("b");

        Nonterminal B = grammar.AddNonterminal("B");

        Rule S = grammar.AddRule("S");
        Rule A = grammar.AddRule("A");

        S.AddProduction(A.Nonterminal, B);
        S.AddProduction(a);

        A.AddProduction(b);

        TestLogger logger = new();
        UnreachableSymbolsStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual("S = 'a'", grammar.ToString());
    }
}