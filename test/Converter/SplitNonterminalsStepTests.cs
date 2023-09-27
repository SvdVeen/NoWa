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
        CFG grammar = new();

        // Add rule S = A B C ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("B"), Nonterminal.Get("C")));

        // Add rule A = 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        // Add rule B = 'b' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));

        // Add rule C = 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

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
        CFG grammar = new();

        // Add rule S = A B C D E ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("B"), Nonterminal.Get("C"), Nonterminal.Get("D"), Nonterminal.Get("E")));

        // Add rule A = 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        // Add rule B = 'b' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));

        // Add rule C = 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

        // Add rule D = 'd' ;
        grammar.AddProduction(new(Nonterminal.Get("D"), Terminal.Get("d")));

        // Add rule E = 'e' ;
        grammar.AddProduction(new(Nonterminal.Get("E"), Terminal.Get("e")));

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
        CFG grammar = new();

        // Add rule S = S1 | S2 ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("S1")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("S2")));

        // Add rule S1 = A B C D ;
        grammar.AddProduction(new(Nonterminal.Get("S1"), Nonterminal.Get("A"), Nonterminal.Get("B"), Nonterminal.Get("C"), Nonterminal.Get("D")));

        // Add rule S2 = A C D ;
        grammar.AddProduction(new(Nonterminal.Get("S2"), Nonterminal.Get("A"), Nonterminal.Get("C"), Nonterminal.Get("D")));

        // Add rule A = 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        // Add rule B = 'b' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));

        // Add rule C = 'C' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

        // Add rule D = 'd' ;
        grammar.AddProduction(new(Nonterminal.Get("D"), Terminal.Get("d")));

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
        CFG grammar = new();

        // Add rule S = A ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A")));

        // Add rule A = 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

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
        CFG grammar = new();

        // Add rule S = S1 | S2 ;
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("S1")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("S2")));

        // Add rule S1 = A B C D ;
        grammar.AddProduction(new(Nonterminal.Get("S1"), Nonterminal.Get("A"), Nonterminal.Get("B"), Nonterminal.Get("C"), Nonterminal.Get("D")));

        // Add rule S2 = A C D ;
        grammar.AddProduction(new(Nonterminal.Get("S2"), Nonterminal.Get("A"), Nonterminal.Get("C"), Nonterminal.Get("D")));

        // Add rule A = 'a' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        // Add rule B = 'b' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));

        // Add rule C = 'C' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

        // Add rule D = 'd' ;
        grammar.AddProduction(new(Nonterminal.Get("D"), Terminal.Get("d")));

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