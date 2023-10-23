using NoWa.Common;

namespace NoWa.Converter.CFGs.Test;

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
        CFG grammar = new();

        // Add rule E = T | E '+' T ;
        grammar.AddProduction(new(Nonterminal.Get("E"), Nonterminal.Get("T")));
        grammar.AddProduction(new(Nonterminal.Get("E"), Nonterminal.Get("E"), Terminal.Get("+"), Nonterminal.Get("T")));

        // Add rule T = F | T '*' F ;
        grammar.AddProduction(new(Nonterminal.Get("T"), Nonterminal.Get("F")));
        grammar.AddProduction(new(Nonterminal.Get("T"), Nonterminal.Get("T"), Terminal.Get("*"), Nonterminal.Get("F")));

        // Add rule F = I | ( E ) ;
        grammar.AddProduction(new(Nonterminal.Get("F"), Nonterminal.Get("I")));
        grammar.AddProduction(new(Nonterminal.Get("F"), Terminal.Get("("), Nonterminal.Get("E"), Terminal.Get(")")));

        // Add rule I = 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;
        grammar.AddProduction(new(Nonterminal.Get("I"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("I"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("I"), Nonterminal.Get("I"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("I"), Nonterminal.Get("I"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("I"), Nonterminal.Get("I"), Terminal.Get("0")));
        grammar.AddProduction(new(Nonterminal.Get("I"), Nonterminal.Get("I"), Terminal.Get("1")));

        UnitProductionsStep step = new(new TestLogger());
        step.Convert(grammar);

        Assert.AreEqual(
            $"E = E '+' T | T '*' F | '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"T = T '*' F | '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"F = '(' E ')' | 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;{Environment.NewLine}" +
            $"I = 'a' | 'b' | I 'a' | I 'b' | I '0' | I '1' ;", grammar.ToString());
    }
}