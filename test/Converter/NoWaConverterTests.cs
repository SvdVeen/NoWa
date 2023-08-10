using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the NoWaConverter
/// </summary>
[TestClass]
[TestCategory("NoWa.Converter")]
public partial class NoWaConverterTests
{
    /// <summary>
    /// Tests the adding of a start rule.
    /// </summary>
    [TestMethod]
    public void AddStartRuleTest()
    {
        Grammar grammar = new();
        // Add rule A = a ;
        Rule rule = grammar.AddRule("A");
        rule.AddExpression(grammar.AddTerminal("a"));

        // Add rule B = A | 'b' ;
        rule = grammar.AddRule("B");
        rule.AddExpression(grammar.AddNonterminal("A"));
        rule.AddExpression(grammar.AddTerminal("b"));

        NoWaConverter.AddStartRule(grammar);

        Assert.AreEqual("START = A ;\r\nA = 'a' ;\r\nB = A | 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the eliminating of ε-productions.
    /// </summary>
    [TestMethod]
    public void EliminateEmptyStringProductionsTestA()
    {
        Grammar grammar = new();
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");

        // Add rule S = A B ;
        Rule rule = grammar.AddRule("S");
        rule.AddExpression(A, B);

        // add rule A = 'a' A A | '' ;
        rule = grammar.AddRule("A");
        rule.AddExpression(a, A, A);
        rule.AddExpression(EmptyString.Instance);

        // Add rule B = 'b' B B | '' ;
        rule = grammar.AddRule("B");
        rule.AddExpression(b, B, B);
        rule.AddExpression(EmptyString.Instance);
        
        NoWaConverter.EliminateEmptyStringProductions(grammar);

        Assert.AreEqual("S = A B | A | B ;\r\nA = 'a' A A | 'a' A | 'a' ;\r\nB = 'b' B B | 'b' B | 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the eliminating of ε-productions.
    /// </summary>
    [TestMethod]
    public void EliminateEmptyStringProductionsTestB()
    {
        Grammar grammar = new();
        Nonterminal A = grammar.AddNonterminal("A");
        Nonterminal B = grammar.AddNonterminal("B");
        Nonterminal C = grammar.AddNonterminal("C");
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.AddTerminal("b");
        Terminal c = grammar.AddTerminal("c");

        // Add rule S = A 'b' B | C;
        Rule rule = grammar.AddRule("S");
        rule.AddExpression(A, b, B);
        rule.AddExpression(C);

        // add rule B = A A | A C ;
        rule = grammar.AddRule("B");
        rule.AddExpression(A, A);
        rule.AddExpression(A, C);

        // Add rule C = 'b' | 'c' ;
        rule = grammar.AddRule("C");
        rule.AddExpression(b);
        rule.AddExpression(c);

        // Add rule A = 'a' | '' ;
        rule = grammar.AddRule("A");
        rule.AddExpression(a);
        rule.AddExpression(EmptyString.Instance);

        NoWaConverter.EliminateEmptyStringProductions(grammar);

        Assert.AreEqual("S = A 'b' B | A 'b' | 'b' B | 'b' | C ;\r\nB = A A | A | A C | C ;\r\nC = 'b' | 'c' ;\r\nA = 'a' ;", grammar.ToString());
    }
}