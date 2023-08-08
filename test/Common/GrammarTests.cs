namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Grammar"/> class.
/// </summary>
[TestClass]
[TestCategory($"{nameof(NoWa)}.{nameof(Common)}")]
public class GrammarTests
{
    /// <summary>
    /// Tests an empty grammar.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestEmptyGrammar)}")]
    public void TestEmptyGrammar()
    {
        Grammar grammar = new();
        Assert.AreEqual(0, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreEqual("Empty grammar", grammar.ToString(), "ToString does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.GetRule(int)"/> function for a rule index outside the bounds of the rule list.
    /// </summary>
    /// <param name="index">The index to get.</param>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestGetRuleOutOfRange)}")]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestGetRuleOutOfRange(int index)
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.GetRule(index));
    }

    /// <summary>
    /// Tests the <see cref="Grammar.GetRule(string)"/> function.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestGetRuleString)}")]
    public void TestGetRuleString()
    {
        Grammar grammar = new();
        Rule rule = grammar.AddRule("a");
        Assert.AreSame(rule, grammar.GetRule("a"));
    }

    /// <summary>
    /// Tests the <see cref="Grammar.GetRule(string)"/> function for a rule that does not exist.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestGetRuleStringNotExists)}")]
    public void TestGetRuleStringNotExists()
    {
        Grammar grammar = new();
        _ = Assert.ThrowsException<KeyNotFoundException>(() => grammar.GetRule("a"));
    }

    /// <summary>
    /// Tests the <see cref="Grammar.GetRule(Nonterminal)"/> function.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestGetRuleNonterminal)}")]
    public void TestGetRuleNonterminal()
    {
        Grammar grammar = new();
        Nonterminal a = new("a");
        Rule rule = new(a);
        grammar.AddRule(rule);
        Assert.AreSame(rule, grammar.GetRule(a));
    }

    /// <summary>
    /// Tests the <see cref="Grammar.GetRule(Nonterminal)"/> function for a nonterminal that is not in the grammar.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestGetRuleNonterminalNotExists)}")]
    public void TestGetRuleNonterminalNotExists()
    {
        Grammar grammar = new();
        _ = Assert.ThrowsException<KeyNotFoundException>(() => grammar.GetRule(new Nonterminal("a")));
    }

    /// <summary>
    /// Tests the <see cref="Grammar.AddRule(Rule)"/> function.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestAddRule)}")]
    public void TestAddRule()
    {
        Grammar grammar = new();
        Rule rule = new(new("a"));
        grammar.AddRule(rule);
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.AddRule(Rule)"/> function when adding a rule with a nonterminal that already has a rule.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestAddRuleAlreadyExists)}")]
    public void TestAddRuleAlreadyExists()
    {
        Grammar grammar = new();
        grammar.AddRule(new Rule(new("a")));
        Rule rule = new(new("a"));
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.AddRule(rule), "Adding the rule did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount);
    }

    /// <summary>
    /// Tests the <see cref="Grammar.AddRule(string)"/> function.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestAddRuleNonterminal)}")]
    public void TestAddRuleNonterminal()
    {
        Grammar grammar = new();
        Rule rule = grammar.AddRule("a");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.AddRule(string)"/> function for a nonterminal that already has a rule.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestAddRuleNonterminalAlreadyExists)}")]
    public void TestAddRuleNonterminalAlreadyExists()
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.AddRule("a"), "Adding the rule did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount);
    }

    /// <summary>
    /// Tests the <see cref="Grammar.InsertRule(int, Rule)"/> function.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestInsertRule)}")]
    public void TestInsertRule()
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        Rule rule = new(new("b"));
        grammar.InsertRule(0, rule);
        Assert.AreEqual(2, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.InsertRule(int, Rule)"/> function for a rule with a nonterminal that already has a rule.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestInsertRuleAlreadyExists)}")]
    public void TestInsertRuleAlreadyExists()
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        Rule rule = new(new("a"));
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.InsertRule(0, rule), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.InsertRule(int, Rule)"/> function for an index out of range.
    /// </summary>
    /// <param name="index"></param>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestInsertRuleOutOfRange)}")]
    [DataTestMethod]
    [DataRow(-1)]
    [DataRow(1)]
    public void TestInsertRuleOutOfRange(int index)
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.InsertRule(index, new Rule(new("b"))), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.InsertRule(int, Rule)"/> function.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestInsertRuleNonTerminal)}")]
    public void TestInsertRuleNonTerminal()
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        Rule rule = grammar.InsertRule(0, "b");
        Assert.AreEqual(2, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.InsertRule(int, Rule)"/> function for a rule with a nonterminal that already has a rule.
    /// </summary>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestInsertRuleNonTerminalAlreadyExists)}")]
    public void TestInsertRuleNonTerminalAlreadyExists()
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.InsertRule(0, "a"), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Grammar.InsertRule(int, Rule)"/> function for an index out of range.
    /// </summary>
    /// <param name="index">The index to insert the rule at.</param>
    [TestMethod($"{nameof(Grammar)}.{nameof(TestInsertRuleNonTerminalOutOfRange)}")]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestInsertRuleNonTerminalOutOfRange(int index)
    {
        Grammar grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.InsertRule(index, "b"), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }
}