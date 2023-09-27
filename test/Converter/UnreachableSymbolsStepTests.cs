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
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Nonterminal S = grammar.AddNonterminal("S");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");

        // Add rule S = A B ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B);
        rule.AddProduction(a);

        // Add rule A = 'a' | 'b'
        rule = grammar.AddRule("A");
        rule.AddProduction(a);
        rule.AddProduction(b);

        UnreachableSymbolsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual("S = 'a' ;", grammar.ToString());
        Assert.AreEqual(1, grammar.Nonterminals.Count);
        Assert.AreSame(S, grammar.Nonterminals[0]);
        Assert.AreEqual(1, grammar.Terminals.Count);
        Assert.AreSame(a, grammar.Terminals[0]);
    }

    /// <summary>
    /// An additional, more complicated test of the <see cref="UnreachableSymbolsStep"/>.
    /// </summary>
    [TestMethod]
    public void TestManyUnreachableSymbols()
    {
        CFG grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Terminal d = grammar.AddTerminal("d");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");

        // Add rule S = A B C | A | 'a' ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B, C);
        rule.AddProduction(A);
        rule.AddProduction(a);

        // Add rule A = C | 'a' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(C);
        rule.AddProduction(a);

        // Add rule C = 'c' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        // Add rule D = 'd' ;
        rule = grammar.AddRule("D");
        rule.AddProduction(d);

        UnreachableSymbolsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A | 'a' ;{Environment.NewLine}" +
            $"A = C | 'a' ;{Environment.NewLine}" +
            $"C = 'c' ;", grammar.ToString());
        Assert.AreEqual(3, grammar.RuleCount);
        Assert.AreEqual(3, grammar.Nonterminals.Count);
        Assert.AreEqual(2, grammar.Terminals.Count);
    }
}