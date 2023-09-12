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

        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");

        Nonterminal B = grammar.AddNonterminal("B");

        Rule S = grammar.AddRule("S");
        Rule A = grammar.AddRule("A");

        S.AddProduction(A.Nonterminal, B);
        S.AddProduction(a);

        A.AddProduction(b);

        TestLogger logger = new();
        UnreachableSymbolsStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual("S = 'a' ;", grammar.ToString());
        Assert.AreEqual(1, grammar.NonterminalCount);
        Assert.AreSame(S.Nonterminal, grammar.GetNonterminal(0));
        Assert.AreEqual(1, grammar.TerminalCount);
        Assert.AreSame(a, grammar.GetTerminal(0));
    }

    /// <summary>
    /// An additional, more complicated test of the <see cref="UnreachableSymbolsStep"/>.
    /// </summary>
    [TestMethod]
    public void TestManyUnreachableSymbols()
    {
        Grammar grammar = new();

        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Terminal d = grammar.AddTerminal("d");
        
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");
        Nonterminal D = grammar.AddNonterminal("D");
        Nonterminal E = grammar.AddNonterminal("E");
        Nonterminal S = grammar.AddNonterminal("S");

        Rule rS = grammar.AddRule("S");
        rS.AddProduction(A, B, C);
        rS.AddProduction(A);
        rS.AddProduction(a);

        Rule rA = grammar.AddRule("A");
        rA.AddProduction(C);
        rA.AddProduction(a);

        Rule rC = grammar.AddRule("C");
        rC.AddProduction(c);

        Rule rD = grammar.AddRule("D");
        rD.AddProduction(d);

        TestLogger logger = new();
        UnreachableSymbolsStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual($"S = A | 'a' ;{Environment.NewLine}" +
            $"A = C | 'a' ;{Environment.NewLine}" +
            $"C = 'c' ;", grammar.ToString());
        Assert.AreEqual(3, grammar.RuleCount);
        Assert.AreEqual(3, grammar.NonterminalCount);
        Assert.AreEqual(2, grammar.TerminalCount);
    }
}