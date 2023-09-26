using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="SeparateTerminalsStep"/>.
/// </summary>
[TestClass]
public class SeparateTerminalsStepTests
{
    /// <summary>
    /// Tests the separating of terminals.
    /// </summary>
    [TestMethod]
    public void TestSeparateTerminals()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Terminal d = grammar.AddTerminal("d");
        Terminal e = grammar.AddTerminal("e");
        Nonterminal B = grammar.AddNonterminal("B");

        // Add rule A = 'a' B | 'c' ;
        Rule rule = grammar.AddRule("A");
        rule.AddProduction(a, B);
        rule.AddProduction(c);

        // Add rule B = 'b' | 'd' 'e' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);
        rule.AddProduction(d, e);

        SeparateTerminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"A = T-a B | 'c' ;{Environment.NewLine}" +
            $"B = 'b' | T-d T-e ;{Environment.NewLine}" +
            $"T-a = 'a' ;{Environment.NewLine}" +
            $"T-d = 'd' ;{Environment.NewLine}" +
            $"T-e = 'e' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the preservation of grammars where no terminals need to be separated.
    /// </summary>
    [TestMethod]
    public void TestNoTerminalsToSeparate()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");

        // Add rule A = 'a' | 'b' ;
        Rule rule = grammar.AddRule("A");
        rule.AddProduction(a);
        rule.AddProduction(b);

        // Add rule C = 'c' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        SeparateTerminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"A = 'a' | 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the separation of terminals if they need to be separated from multiple bodies.
    /// </summary>
    [TestMethod]
    public void TestSeparateTerminalsTwice()
    {
        Grammar grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");

        // Add rule A = 'a' B | 'a' C ;
        Rule rule = grammar.AddRule("A");
        rule.AddProduction(a, B);
        rule.AddProduction(a, C);
        
        // Add rule B = 'b' | 'a' C ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);
        rule.AddProduction(a, C);

        // Add rule C = 'c' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(c);

        SeparateTerminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"A = T-a B | T-a C ;{Environment.NewLine}" +
            $"B = 'b' | T-a C ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"T-a = 'a' ;", grammar.ToString());
    }
}