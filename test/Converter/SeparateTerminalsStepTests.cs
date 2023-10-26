using NoWa.Common;

namespace NoWa.Converter.Test;

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
        CFG grammar = new();

        // Add rule A = 'a' B | 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a"), Nonterminal.Get("B")));
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("c")));

        // Add rule B = 'b' | 'd' 'e' ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("d"), Terminal.Get("e")));

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
        CFG grammar = new();

        // Add rule A = 'a' | 'b' ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("b")));

        // Add rule C = 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

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
        CFG grammar = new();

        // Add rule A = 'a' B | 'a' C ;
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a"), Nonterminal.Get("B")));
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a"), Nonterminal.Get("C")));

        // Add rule B = 'b' | 'a' C ;
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("a"), Nonterminal.Get("C")));

        // Add rule C = 'c' ;
        grammar.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

        SeparateTerminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"A = T-a B | T-a C ;{Environment.NewLine}" +
            $"B = 'b' | T-a C ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"T-a = 'a' ;", grammar.ToString());
    }
}