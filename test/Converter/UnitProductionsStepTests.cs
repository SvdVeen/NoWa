using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="UnitProductionsStep"/>.
/// </summary>
[TestClass]
public class UnitProductionsStepTests
{
    /// <summary>
    /// Tests the <see cref="EmptyStringStep"/>.
    /// </summary>
    /// <remarks>
    /// This unit test is based on the example given by Hopcroft, Motwani, and Ullman on pages 268-271 of the 2013 edition
    /// of "Introduction to Automata Theory, Languages, and Computation: Pearson New International Edition" 
    /// </remarks>
    [TestMethod]
    public void TestUnitProductions()
    {
        Grammar grammar = new();

        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal zero = grammar.AddTerminal("0");
        Terminal one = grammar.AddTerminal("1");
        Terminal openParen = grammar.AddTerminal("(");
        Terminal closeParen = grammar.AddTerminal(")");
        Terminal times = grammar.AddTerminal("*");
        Terminal plus = grammar.AddTerminal("+");

        Rule I = grammar.AddRule("I");
        Rule F = grammar.AddRule("F");
        Rule T = grammar.AddRule("T");
        Rule E = grammar.AddRule("E");

        I.AddProduction(a);
        I.AddProduction(b);
        I.AddProduction(I.Nonterminal, a);
        I.AddProduction(I.Nonterminal, b);
        I.AddProduction(I.Nonterminal, zero);
        I.AddProduction(I.Nonterminal, one);

        F.AddProduction(I.Nonterminal);
        F.AddProduction(openParen, E.Nonterminal, closeParen);

        T.AddProduction(F.Nonterminal);
        T.AddProduction(T.Nonterminal, times, F.Nonterminal);

        E.AddProduction(T.Nonterminal);
        E.AddProduction(E.Nonterminal, plus, T.Nonterminal);

        TestLogger logger = new TestLogger();
        UnitProductionsStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual(
            $"E = E '+' T | T '*' F | '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"T = T '*' F | '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"F = '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"I = 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;", grammar.ToString());
    }
}