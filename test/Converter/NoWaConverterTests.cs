using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the NoWaConverter
/// </summary>
[TestClass]
public class NoWaConverterTests
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
}