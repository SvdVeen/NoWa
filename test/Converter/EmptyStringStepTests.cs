using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="EmptyStringStep"/>.
/// </summary>
[TestClass]
public class EmptyStringStepTests
{
    /// <summary>
    /// Tests the <see cref="EmptyStringStep.Convert(CFG)"/> function.
    /// </summary>
    /// <remarks>
    /// This unit test is based on the example given by Hopcroft, Motwani, and Ullman on pages 266-267 of the 2013 edition
    /// of "Introduction to Automata Theory, Languages, and Computation: Pearson New International Edition" 
    /// </remarks>
    [TestMethod]
    public void TestRemoveEmptyStringProductionsA()
    {
        CFG grammar = new();
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");

        // Add rule S = A B ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B);

        // add rule A = 'a' A A | '' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a, A, A);
        rule.AddProduction(EmptyString.Instance);

        // Add rule B = 'b' B B | '' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b, B, B);
        rule.AddProduction(EmptyString.Instance);

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A B | B | A ;{Environment.NewLine}" +
            $"A = 'a' A A | 'a' A | 'a' ;{Environment.NewLine}" +
            $"B = 'b' B B | 'b' B | 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the <see cref="EmptyStringStep.Convert(CFG)"/> function.
    /// </summary>
    /// <remarks>
    /// This unit test is based on the example given on https://en.wikipedia.org/wiki/Chomsky_normal_form (2023-08-15).
    /// </remarks>
    [TestMethod]
    public void TestRemoveEmptyStringProductionsB()
    {
        CFG grammar = new();
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");

        // Add rule S = A 'b' B | C;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, b, B);
        rule.AddProduction(C);

        // add rule B = A A | A C ;
        rule = grammar.AddRule("B");
        rule.AddProduction(A, A);
        rule.AddProduction(A, C);

        // Add rule C = 'b' | 'c' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(b);
        rule.AddProduction(c);

        // Add rule A = 'a' | '' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(a);
        rule.AddProduction(EmptyString.Instance);

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A 'b' B | 'b' B | 'b' | A 'b' | C ;{Environment.NewLine}" +
            $"B = A A | A | A C | C ;{Environment.NewLine}" +
            $"C = 'b' | 'c' ;{Environment.NewLine}" +
            $"A = 'a' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests whether the step removes empty rules (rules that only produce empty strings).
    /// </summary>
    [TestMethod]
    public void TestRemoveEmptyRule()
    {
        CFG grammar = new();
        Terminal b = grammar.AddTerminal("b");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        
        // Add rule S = A B ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B);

        // Add rule A = '' '' | '' ;
        rule = grammar.AddRule("A");
        rule.AddProduction(EmptyString.Instance, EmptyString.Instance);
        rule.AddProduction(EmptyString.Instance);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = B ;{Environment.NewLine}" +
            $"B = 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests whether the step removes empty rules several levels deep (those that contain only rules that would only produce empty strings).
    /// </summary>
    [TestMethod]
    public void TestRemoveNestedEmptyRule()
    {
        CFG grammar = new();
        Terminal b = grammar.AddTerminal("b");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");

        // Add rule S = A B ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B);

        // Add rule A = C ;
        rule = grammar.AddRule("A");
        rule.AddProduction(C);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        // Add rule C = '' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(EmptyString.Instance);

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = B ;{Environment.NewLine}" +
            $"B = 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the step when there is nothing to remove.
    /// </summary>
    [TestMethod]
    public void TestRemoveNothing()
    {
        CFG grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");

        // Add rule S = 'a' | 'b' ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(a);
        rule.AddProduction(b);

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = 'a' | 'b' ;", grammar.ToString());
    }

    /// <summary>
    ///  Tests performing the step twice.
    /// </summary>
    [TestMethod]
    public void TestPerformStepTwice()
    {
        CFG grammar = new();
        Terminal b = grammar.AddTerminal("b");
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");

        // Add rule S = A B ;
        Rule rule = grammar.AddRule("S");
        rule.AddProduction(A, B);

        // Add rule A = C ;
        rule = grammar.AddRule("A");
        rule.AddProduction(C);

        // Add rule B = 'b' ;
        rule = grammar.AddRule("B");
        rule.AddProduction(b);

        // Add rule C = '' ;
        rule = grammar.AddRule("C");
        rule.AddProduction(EmptyString.Instance);

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = B ;{Environment.NewLine}" +
            $"B = 'b' ;", grammar.ToString());
    }
}