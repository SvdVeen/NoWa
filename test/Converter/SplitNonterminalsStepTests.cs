using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="SplitNonterminalsStep"/>.
/// </summary>
[TestClass]
public class SplitNonterminalsStepTests
{
    /// <summary>
    /// Tests the splitting of a single rule with three nonterminals.
    /// </summary>
    [TestMethod]
    public void TestSplitThree()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");

        // Add rule S = A B C ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B, C);

        // Add rule A = 'a' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        // Add rule C = 'c' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        SplitNonterminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A B-C ;{Environment.NewLine}" +
            $"A = 'a' ;{Environment.NewLine}" +
            $"B = 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"B-C = B C ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the splitting of a single rule with five nonterminals.
    /// </summary>
    [TestMethod]
    public void TestSplitFive()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Terminal d = grammar.AddTerminal("d");
        Terminal e = grammar.AddTerminal("e");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");
        Nonterminal D = grammar.AddNonterminal("D");
        Nonterminal E = grammar.AddNonterminal("E");

        // Add rule S = A B C D E ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B, C, D, E);

        // Add rule A = 'a' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        // Add rule C = 'c' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        // Add rule D = 'd' ;
        rule = grammar.AddRule("D");
        rule.AddProduction(d);

        // Add rule E = 'e' ;
        rule = grammar.AddRule("E");
        rule.AddProduction(e);

        SplitNonterminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A B-C-D-E ;{Environment.NewLine}" +
            $"A = 'a' ;{Environment.NewLine}" +
            $"B = 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"D = 'd' ;{Environment.NewLine}" +
            $"E = 'e' ;{Environment.NewLine}" +
            $"D-E = D E ;{Environment.NewLine}" +
            $"C-D-E = C D-E ;{Environment.NewLine}" +
            $"B-C-D-E = B C-D-E ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the splitting of multiple productions.
    /// </summary>
    [TestMethod]
    public void TestSplitMultipleProductions()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Terminal d = grammar.AddTerminal("d");
        Nonterminal S1 = grammar.AddNonterminal("S1");
        Nonterminal S2 = grammar.AddNonterminal("S2");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");
        Nonterminal D = grammar.AddNonterminal("D");

        // Add rule S = S1 | S2 ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(S1);
        rule.AddProduction(S2);

        // Add rule S1 = A B C D ;
        rule = grammar.AddRule("S1");
        rule.AddProduction(A, B, C, D);

        // Add rule S2 = A C D ;
        rule = grammar.AddRule("S2");
        rule.AddProduction(A, C, D);

        // Add rule A = 'a' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        // Add rule C = 'C' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        // Add rule D = 'd' ;
        rule = grammar.AddRule("D");
        rule.AddProduction(d);

        SplitNonterminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = S1 | S2 ;{Environment.NewLine}" +
            $"S1 = A B-C-D ;{Environment.NewLine}" +
            $"S2 = A C-D ;{Environment.NewLine}" +
            $"A = 'a' ;{Environment.NewLine}" +
            $"B = 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"D = 'd' ;{Environment.NewLine}" +
            $"C-D = C D ;{Environment.NewLine}" +
            $"B-C-D = B C-D ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the step when nothing needs to be split.
    /// </summary>
    [TestMethod]
    public void TestSplitNone()
    {
        Grammar grammar = new Grammar();
        Terminal a = grammar.AddTerminal("a");
        Nonterminal A = grammar.AddNonterminal("A");

        // Add rule S = A ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A);

        // Add rule A = 'a' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a);

        SplitNonterminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A ;{Environment.NewLine}" +
            $"A = 'a' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests whether the result remains the same if the step is performed twice.
    /// </summary>
    [TestMethod]
    public void TestPerformStepTwice()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Terminal d = grammar.AddTerminal("d");
        Nonterminal S1 = grammar.AddNonterminal("S1");
        Nonterminal S2 = grammar.AddNonterminal("S2");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");
        Nonterminal D = grammar.AddNonterminal("D");

        // Add rule S = S1 | S2 ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(S1);
        rule.AddProduction(S2);

        // Add rule S1 = A B C D ;
        rule = grammar.AddRule("S1");
        rule.AddProduction(A, B, C, D);

        // Add rule S2 = A C D ;
        rule = grammar.AddRule("S2");
        rule.AddProduction(A, C, D);

        // Add rule A = 'a' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        // Add rule C = 'C' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        // Add rule D = 'd' ;
        rule = grammar.AddRule("D");
        rule.AddProduction(d);

        SplitNonterminalsStep step = new(new TestLogger());
        step.Convert(grammar);
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = S1 | S2 ;{Environment.NewLine}" +
            $"S1 = A B-C-D ;{Environment.NewLine}" +
            $"S2 = A C-D ;{Environment.NewLine}" +
            $"A = 'a' ;{Environment.NewLine}" +
            $"B = 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"D = 'd' ;{Environment.NewLine}" +
            $"C-D = C D ;{Environment.NewLine}" +
            $"B-C-D = B C-D ;", grammar.ToString());
    }
}