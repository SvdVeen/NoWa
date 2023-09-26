using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="UnitProductionsStep"/>.
/// </summary>
[TestClass]
public class UnitProductionsStepTests
{
    /// <summary>
    /// Tests the <see cref="UnitProductionsStep"/>.
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
        Nonterminal E = grammar.AddNonterminal("E");
        Nonterminal T = grammar.AddNonterminal("T");
        Nonterminal F = grammar.AddNonterminal("F");
        Nonterminal I = grammar.AddNonterminal("I");

        // Add rule E = T | E '+' T ;
        Rule rule = grammar.AddRule("E");
        rule.AddProduction(T);
        rule.AddProduction(E, plus, T);

        // Add rule T = F | T '*' F ;
        rule = grammar.AddRule("T");
        rule.AddProduction(F);
        rule.AddProduction(T, times, F);

        // Add rule F = I | ( E ) ;
        rule = grammar.AddRule("F");
        rule.AddProduction(I);
        rule.AddProduction(openParen, E, closeParen);

        // Add rule I = 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;
        rule = grammar.AddRule("I");
        rule.AddProduction(a);
        rule.AddProduction(b);
        rule.AddProduction(I, a);
        rule.AddProduction(I, b);
        rule.AddProduction(I, zero);
        rule.AddProduction(I, one);

        UnitProductionsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"E = E '+' T | T '*' F | '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"T = T '*' F | '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"F = '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"I = 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;", grammar.ToString());
    }
}