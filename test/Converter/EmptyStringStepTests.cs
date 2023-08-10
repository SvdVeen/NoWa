using NoWa.Common;

namespace NoWa.Converter.Tests;

/// <summary>
/// Contains unit tests for the <see cref="EmptyStringStep"/>.
/// </summary>
[TestClass]
public class EmptyStringStepTests
{
    /// <summary>
    /// Tests the <see cref="EmptyStringStep.Convert(Grammar)"/> function.
    /// </summary>
    /// <remarks>
    /// This unit test is based on the example given by TODO: SOURCE.
    /// </remarks>
    [TestMethod]
    public void TestRemoveEmptyStringProductionsA()
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

        TestLogger logger = new TestLogger();
        EmptyStringStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual("S = A B | A | B ;\r\nA = 'a' A A | 'a' A | 'a' ;\r\nB = 'b' B B | 'b' B | 'b' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests the <see cref="EmptyStringStep.Convert(Grammar)"/> function.
    /// </summary>
    /// <remarks>
    /// This unit test is based on the example given by TODO: SOURCE (wiki).
    /// </remarks>
    [TestMethod]
    public void TestRemoveEmptyStringProductionsB()
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

        TestLogger logger = new();
        EmptyStringStep step = new(logger);
        step.Convert(grammar);

        Assert.AreEqual("S = A 'b' B | A 'b' | 'b' B | 'b' | C ;\r\nB = A A | A | A C | C ;\r\nC = 'b' | 'c' ;\r\nA = 'a' ;", grammar.ToString());
    }
}