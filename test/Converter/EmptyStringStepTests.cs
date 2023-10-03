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

        // Add rule S = A B ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("B")));

        // add rule A = 'a' A A | '' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a"), Nonterminal.Get("A"), Nonterminal.Get("A")));
        grammar.AddProduction(new(Nonterminal.Get("A"), EmptyString.Instance));

        // Add rule B = 'b' B B | '' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b"), Nonterminal.Get("B"), Nonterminal.Get("B")));
        grammar.AddProduction(new(Nonterminal.Get("B"), EmptyString.Instance));

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

        // Add rule S = A 'b' B | C;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Terminal.Get("b"), Nonterminal.Get("B")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("C")));

        // add rule B = A A | A C ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Nonterminal.Get("A"), Nonterminal.Get("A")));
        grammar.AddProduction(new(Nonterminal.Get("B"), Nonterminal.Get("A"), Nonterminal.Get("C")));

        // Add rule C = 'b' | 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

        // Add rule A = 'a' | '' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("A"), EmptyString.Instance));

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A 'b' B | C | 'b' B | A 'b' | 'b' ;{Environment.NewLine}" +
            $"B = A A | A C | A | C ;{Environment.NewLine}" +
            $"C = 'b' | 'c' ;{Environment.NewLine}" +
            $"A = 'a' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the step when there is nothing to remove.
    /// </summary>
    [TestMethod]
    public void TestRemoveNothing()
    {
        CFG grammar = new();

        // Add rule S = 'a' | 'b' ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("b")));

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

        // Add rule S = A B ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("B")));

        // add rule A = 'a' A A | '' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a"), Nonterminal.Get("A"), Nonterminal.Get("A")));
        grammar.AddProduction(new(Nonterminal.Get("A"), EmptyString.Instance));

        // Add rule B = 'b' B B | '' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b"), Nonterminal.Get("B"), Nonterminal.Get("B")));
        grammar.AddProduction(new(Nonterminal.Get("B"), EmptyString.Instance));

        EmptyStringStep step = new(new TestLogger());
        step.Convert(grammar);
        step.Convert(grammar);

        Assert.AreEqual(
            $"S = A B | B | A ;{Environment.NewLine}" +
            $"A = 'a' A A | 'a' A | 'a' ;{Environment.NewLine}" +
            $"B = 'b' B B | 'b' B | 'b' ;", grammar.ToString());
    }
}