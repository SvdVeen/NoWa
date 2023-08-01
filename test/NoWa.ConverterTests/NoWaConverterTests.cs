using NoWa.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoWa.Common;
using System.Text.RegularExpressions;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the NoWaConverter
/// </summary>
[TestClass]
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
        rule.AddExpression(grammar.GetOrCreateTerminal("a"));

        // Add rule B = A | 'b' ;
        rule = grammar.AddRule("B");
        rule.AddExpression(grammar.GetOrCreateNonterminal("A"));
        rule.AddExpression(grammar.GetOrCreateTerminal("b"));

        NoWaConverter.AddStartRule(grammar);

        Assert.AreEqual("START = A ;\r\nA = 'a' ;\r\nB = A | 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the eliminating of ε-productions.
    /// </summary>
    [TestMethod]
    public void EliminateEmptyStringProductionsTest()
    {
        Grammar grammar = new();
        Nonterminal A = grammar.GetOrCreateNonterminal("A");
        Nonterminal B = grammar.GetOrCreateNonterminal("B");
        Terminal a = grammar.GetOrCreateTerminal("a");
        Terminal b = grammar.GetOrCreateTerminal("b");

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
}