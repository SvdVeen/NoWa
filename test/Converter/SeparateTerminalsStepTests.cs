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

        Rule RA = grammar.AddRule("A");
        RA.AddProduction(a, B);
        RA.AddProduction(c);

        Rule RB = grammar.AddRule("B");
        RB.AddProduction(b);
        RB.AddProduction(d, e);

        TestLogger logger = new();
        SeparateTerminalsStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual($"A = T-a B | 'c' ;{Environment.NewLine}" +
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

        Rule A = grammar.AddRule("A");
        A.AddProduction(a);
        A.AddProduction(b);

        Rule C = grammar.AddRule("C");
        C.AddProduction(c);

        TestLogger logger = new();
        SeparateTerminalsStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual($"A = 'a' | 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;", grammar.ToString());
    }
}