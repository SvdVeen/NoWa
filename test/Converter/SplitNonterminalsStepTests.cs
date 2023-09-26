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
    public void TestSplitOne()
    {
        Grammar grammar = new();

        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");

        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");

        Rule S = grammar.AddRule("S");
        S.AddProduction(A, B, C);

        Rule RA = grammar.AddRule("A");
        RA.AddProduction(a);

        Rule RB = grammar.AddRule("B");
        RB.AddProduction(b);

        Rule RC = grammar.AddRule("C");
        RC.AddProduction(c);

        SplitNonterminalsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual($"S = A B-C ;{Environment.NewLine}" +
            $"A = 'a' ;{Environment.NewLine}" +
            $"B = 'b' ;{Environment.NewLine}" +
            $"C = 'c' ;{Environment.NewLine}" +
            $"B-C = B C ;", grammar.ToString());
    }
}