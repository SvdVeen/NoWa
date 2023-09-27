﻿namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="CFG"/> class.
/// </summary>
[TestClass]
public class CFGTests
{
    /// <summary>
    /// Tests an empty grammar.
    /// </summary>
    [TestMethod]
    public void TestEmptyGrammar()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreEqual(0, grammar.Nonterminals.Count, "NonterminalCount does not match.");
        Assert.AreEqual(0, grammar.Terminals.Count, "NonterminalCount does not match.");
        Assert.AreEqual("Empty grammar", grammar.ToString(), "ToString does not match.");
    }

    #region Terminals

    /// <summary>
    /// Tests the <see cref="CFG.Terminals"/> property after adding a terminal.
    /// </summary>
    [TestMethod]
    public void TestTerminals()
    {
        CFG grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Terminal b = grammar.Terminals[0];
        Assert.AreSame(a, b);
    }

    /// <summary>
    /// Tests whether <see cref="CFG.Tonterminal(int)"/> throws an exception for an index outside the acceptable range.
    /// </summary>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestGetTerminalIndexOutOfRange(int index)
    {
        CFG grammar = new();
        Terminal a = grammar.AddTerminal("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.Terminals[index]);
    }

    /// <summary>
    /// Tests adding a terminal with the <see cref="CFG.AddTerminal(Terminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddTerminal()
    {
        CFG grammar = new();
        Terminal a = Terminal.Get("a");
        Assert.IsTrue(grammar.AddTerminal(a));
    }

    /// <summary>
    /// Tests adding a terminal twice with the <see cref="CFG.AddTerminal(Terminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddTerminalTwice()
    {
        CFG grammar = new();
        Terminal a = Terminal.Get("a");
        _ = grammar.AddTerminal(a);
        Assert.IsFalse(grammar.AddTerminal(a));
    }

    /// <summary>
    /// Test the <see cref="CFG.AddNonterminal(string)"/> function.
    /// </summary>
    [TestMethod]
    public void TestAddTerminalValue()
    {
        CFG grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Assert.AreSame(a, grammar.AddTerminal("a"));
    }

    /// <summary>
    /// Tests the <see cref="CFG.Terminals"/> property's count after adding and removing various terminals.
    /// </summary>
    [TestMethod]
    public void TestTerminalsCount()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.Terminals.Count);

        _ = grammar.AddTerminal("a");
        Assert.AreEqual(1, grammar.Terminals.Count);

        _ = grammar.AddTerminal("b");
        Assert.AreEqual(2, grammar.Terminals.Count);

        grammar.RemoveTerminalAt(1);
        Assert.AreEqual(1, grammar.Terminals.Count);

        grammar.RemoveTerminalAt(0);
        Assert.AreEqual(0, grammar.Terminals.Count);
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsTerminal(Terminal)"/> method for a terminal that exists in the grammar.
    /// </summary>
    [TestMethod]
    public void TestContainsTerminal()
    {
        CFG grammar = new();
        Terminal a = grammar.AddTerminal("a");
        Assert.IsTrue(grammar.ContainsTerminal(a));
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsTerminal(Terminal)"/> method for a terminal that does not exist in the grammar.
    /// </summary>
    [TestMethod]
    public void TestDoesNotContainTerminal()
    {
        CFG grammar = new();
        _ = grammar.AddTerminal("b");
        Assert.IsFalse(grammar.ContainsTerminal(Terminal.Get("a")));
    }

    #endregion Terminals

    #region Nonterminals

    /// <summary>
    /// Tests the <see cref="CFG.Nonterminals"/> property after adding a nonterminal.
    /// </summary>
    [TestMethod]
    public void TestGetNonterminalIndex()
    {
        CFG grammar = new();
        Nonterminal a = grammar.AddNonterminal("a");
        Nonterminal b = grammar.Nonterminals[0];
        Assert.AreSame(a, b);
    }

    /// <summary>
    /// Tests whether <see cref="CFG.Nonterminals"/> throws an exception for an index outside the acceptable range.
    /// </summary>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestGetNonterminalIndexOutOfRange(int index)
    {
        CFG grammar = new();
        Nonterminal a = grammar.AddNonterminal("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.Nonterminals[index]);
    }

    /// <summary>
    /// Tests adding a nonterminal with the <see cref="CFG.AddNonterminal(Nonterminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddNonterminal()
    {
        CFG grammar = new();
        Nonterminal A = Nonterminal.Get("A");
        Assert.IsTrue(grammar.AddNonterminal(A));
    }

    /// <summary>
    /// Tests adding a nonterminal twice with the <see cref="CFG.AddNonterminal(Nonterminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddNonterminalTwice()
    {
        CFG grammar = new();
        Nonterminal A = Nonterminal.Get("A");
        _ = grammar.AddNonterminal(A);
        Assert.IsFalse(grammar.AddNonterminal(A));
    }

    /// <summary>
    /// Test the <see cref="CFG.AddNonterminal(string)"/> function.
    /// </summary>
    [TestMethod]
    public void TestAddNonterminalValue()
    {
        CFG grammar = new();
        Nonterminal a = grammar.AddNonterminal("a");
        Assert.AreSame(a, grammar.AddNonterminal("a"));
    }

    /// <summary>
    /// Test the <see cref="CFG.AddNonterminal(string)"/> function.
    /// </summary>
    [TestMethod]
    public void TestAddNonterminalAlreadyExists()
    {
        CFG grammar = new();
        Nonterminal a = Nonterminal.Get("a");
        Rule rule = new(a);
        grammar.AddRule(rule);
        Assert.AreSame(rule.Nonterminal, grammar.AddNonterminal("a"));
    }

    /// <summary>
    /// Tests the <see cref="CFG.Nonterminals"/>.Count property after adding and removing various nonterminals.
    /// </summary>
    [TestMethod]
    public void TestNonterminalCount()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.Nonterminals.Count);

        _ = grammar.AddNonterminal("a");
        Assert.AreEqual(1, grammar.Nonterminals.Count);

        _ = grammar.AddNonterminal("b");
        Assert.AreEqual(2, grammar.Nonterminals.Count);

        grammar.RemoveNonterminalAt(1);
        Assert.AreEqual(1, grammar.Nonterminals.Count);

        grammar.RemoveNonterminalAt(0);
        Assert.AreEqual(0, grammar.Nonterminals.Count);
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsNonterminal(Nonterminal)"/> method for a nonterminal that exists in the grammar.
    /// </summary>
    [TestMethod]
    public void TestContainsNonterminal()
    {
        CFG grammar = new();
        Nonterminal A = grammar.AddNonterminal("A");
        Assert.IsTrue(grammar.ContainsNonterminal(A));
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsNonterminal(Nonterminal)"/> method for a nonterminal that does not exist in the grammar.
    /// </summary>
    [TestMethod]
    public void TestDoesNotContainNonterminal()
    {
        CFG grammar = new();
        _ = grammar.AddNonterminal("B");
        Assert.IsFalse(grammar.ContainsNonterminal(Nonterminal.Get("A")));
    }

    #endregion Nonterminals

    #region Rules

    /// <summary>
    /// Tests the <see cref="CFG.GetRule(int)"/> function for a rule index outside the bounds of the rule list.
    /// </summary>
    /// <param name="index">The index to get.</param>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestGetRuleOutOfRange(int index)
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.GetRule(index));
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetRule(string)"/> function.
    /// </summary>
    [TestMethod]
    public void TestGetRuleString()
    {
        CFG grammar = new();
        Rule rule = grammar.AddRule("a");
        Assert.AreSame(rule, grammar.GetRule("a"));
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetRule(string)"/> function for a rule that does not exist.
    /// </summary>
    [TestMethod]
    public void TestGetRuleStringNotExists()
    {
        CFG grammar = new();
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.GetRule("a"));
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetRule(Nonterminal)"/> function.
    /// </summary>
    [TestMethod]
    public void TestGetRuleNonterminal()
    {
        CFG grammar = new();
        Nonterminal a = Nonterminal.Get("a");
        Rule rule = new(a);
        grammar.AddRule(rule);
        Assert.AreSame(rule, grammar.GetRule(a));
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetRule(Nonterminal)"/> function for a nonterminal that is not in the grammar.
    /// </summary>
    [TestMethod]
    public void TestGetRuleNonterminalNotExists()
    {
        CFG grammar = new();
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.GetRule(Nonterminal.Get("a")));
    }

    /// <summary>
    /// Tests the <see cref="CFG.AddRule(Rule)"/> function.
    /// </summary>
    [TestMethod]
    public void TestAddRule()
    {
        CFG grammar = new();
        Rule rule = new(Nonterminal.Get("a"));
        grammar.AddRule(rule);
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.TryGetRule(string, out Rule?)"/> function.
    /// </summary>
    [TestMethod]
    public void TestTryGetRuleString()
    {
        CFG grammar = new();
        Rule rule = new(Nonterminal.Get("a"));
        grammar.AddRule(rule);
        Assert.IsTrue(grammar.TryGetRule("a", out Rule? res), "TryGetRule did not return true.");
        Assert.AreSame(rule, res, "Rules do not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.TryGetRule(string, out Rule?)"/> function for a rule that does not exist.
    /// </summary>
    [TestMethod]
    public void TestTryGetRuleStringNotExists()
    {
        CFG grammar = new();
        Assert.IsFalse(grammar.TryGetRule("a", out Rule? res), "TryGetRule did not return false.");
        Assert.IsNull(res, "Result is not null.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.TryGetRule(Nonterminal, out Rule?)"/> function.
    /// </summary>
    [TestMethod]
    public void TestTryGetRuleNonterminal()
    {
        CFG grammar = new();
        Nonterminal a = Nonterminal.Get("a");
        Rule rule = new(a);
        grammar.AddRule(rule);
        Assert.IsTrue(grammar.TryGetRule(a, out Rule? res), "TryGetRule did not return true.");
        Assert.AreSame(rule, res, "Rules do not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.TryGetRule(Nonterminal, out Rule?)"/> function for a rule that does not exist.
    /// </summary>
    [TestMethod]
    public void TestTryGetRuleNonterminalNotExists()
    {
        CFG grammar = new();
        Assert.IsFalse(grammar.TryGetRule(Nonterminal.Get("a"), out Rule? res), "TryGetRule did not return false.");
        Assert.IsNull(res, "Result is not null.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.AddRule(Rule)"/> function when adding a rule with a nonterminal that already has a rule.
    /// </summary>
    [TestMethod]
    public void TestAddRuleAlreadyExists()
    {
        CFG grammar = new();
        grammar.AddRule(new Rule(Nonterminal.Get("a")));
        Rule rule = new(Nonterminal.Get("a"));
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.AddRule(rule), "Adding the rule did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount);
    }

    /// <summary>
    /// Tests the <see cref="CFG.AddRule(string)"/> function.
    /// </summary>
    [TestMethod]
    public void TestAddRuleNonterminal()
    {
        CFG grammar = new();
        Rule rule = grammar.AddRule("a");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.AddRule(string)"/> function for a nonterminal that already has a rule.
    /// </summary>
    [TestMethod]
    public void TestAddRuleNonterminalAlreadyExists()
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.AddRule("a"), "Adding the rule did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount);
    }

    /// <summary>
    /// Tests the <see cref="CFG.InsertRule(int, Rule)"/> function.
    /// </summary>
    [TestMethod]
    public void TestInsertRule()
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        Rule rule = new(Nonterminal.Get("b"));
        grammar.InsertRule(0, rule);
        Assert.AreEqual(2, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.InsertRule(int, Rule)"/> function for a rule with a nonterminal that already has a rule.
    /// </summary>
    [TestMethod]
    public void TestInsertRuleAlreadyExists()
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        Rule rule = new(Nonterminal.Get("a"));
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.InsertRule(0, rule), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.InsertRule(int, Rule)"/> function for an index out of range.
    /// </summary>
    /// <param name="index"></param>
    [TestMethod]
    [DataTestMethod]
    [DataRow(-1)]
    [DataRow(1)]
    public void TestInsertRuleOutOfRange(int index)
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.InsertRule(index, new Rule(Nonterminal.Get("b"))), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.InsertRule(int, Rule)"/> function.
    /// </summary>
    [TestMethod]
    public void TestInsertRuleNonTerminal()
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        Rule rule = grammar.InsertRule(0, "b");
        Assert.AreEqual(2, grammar.RuleCount, "RuleCount does not match.");
        Assert.AreSame(rule, grammar.GetRule(0), "Rule does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.InsertRule(int, Rule)"/> function for a rule with a nonterminal that already has a rule.
    /// </summary>
    [TestMethod]
    public void TestInsertRuleNonTerminalAlreadyExists()
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentException>(() => grammar.InsertRule(0, "a"), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.InsertRule(int, Rule)"/> function for an index out of range.
    /// </summary>
    /// <param name="index">The index to insert the rule at.</param>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestInsertRuleNonTerminalOutOfRange(int index)
    {
        CFG grammar = new();
        _ = grammar.AddRule("a");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.InsertRule(index, "b"), "The insertion did not throw an exception.");
        Assert.AreEqual(1, grammar.RuleCount, "RuleCount does not match.");
    }

    #endregion Rules

    #region ReplaceSymbol

    /// <summary>
    /// Helper for testing the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function
    /// </summary>
    private static void TestReplaceSymbol(ISymbol symbol, ISymbol newSymbol, bool removeSymbol)
    {
        Nonterminal Ntest = Nonterminal.Get("test");
        Nonterminal Ttest = Nonterminal.Get("test");
        CFG grammar = new();

        Rule rule1 = grammar.AddRule("test1");
        Nonterminal test1 = rule1.Nonterminal;
        rule1.Productions.Add(new() { Ntest, symbol, Ttest });
        rule1.Productions.Add(new() { Ttest, Ttest });

        Rule rule2 = grammar.AddRule("test2");
        Nonterminal test2 = rule2.Nonterminal;
        rule2.Productions.Add(new Expression() { Ttest, Ttest, symbol, symbol });

        grammar.ReplaceSymbol(symbol, newSymbol, removeSymbol);

        rule1 = grammar.GetRule(0);
        Assert.AreSame(test1, rule1.Nonterminal, "The first rule does not have its original nonterminal.");
        ExpressionAssert.AreSameEntries(rule1.Productions[0], "The first rule's first expression does not match the expected symbols.", Ntest, newSymbol, Ttest);
        ExpressionAssert.AreSameEntries(rule1.Productions[1], "The first rule's second expression does not match the expected symbols.", Ttest, Ttest);

        rule2 = grammar.GetRule(1);
        Assert.AreSame(test2, rule2.Nonterminal, "The second rule does not have its original nonterminal.");
        ExpressionAssert.AreSameEntries(rule2.Productions[0], "The second rule's expression does not match the expected symbols." , Ttest, Ttest, newSymbol, newSymbol);

        if (removeSymbol)
        {
            if (symbol is Terminal terminal && newSymbol.Value != symbol.Value)
            {
                Assert.IsFalse(grammar.ContainsTerminal(terminal), "The original terminal was not removed.");
            }
            else if (symbol is Nonterminal nonterminal && newSymbol.Value != symbol.Value)
            {
                Assert.IsFalse(grammar.ContainsNonterminal(nonterminal), "The original nonterminal was not removed.");
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing a <see cref="Terminal"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace the original with.</param>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow("a", "a", true)]
    [DataRow("a", "b", true)]
    [DataRow("a", "a", false)]
    [DataRow("a", "b", false)]
    public void TestReplaceTT(string symbol, string newSymbol, bool removeSymbol)
        => TestReplaceSymbol(Terminal.Get(symbol), Terminal.Get(newSymbol), removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing a <see cref="Terminal"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace the original with.</param>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow("a", "a", true)]
    [DataRow("a", "b", true)]
    [DataRow("a", "a", false)]
    [DataRow("a", "b", false)]
    public void TestReplaceTNT(string symbol, string newSymbol, bool removeSymbol)
        => TestReplaceSymbol(Terminal.Get(symbol), Nonterminal.Get(newSymbol), removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing a <see cref="Terminal"/> with an <see cref="EmptyString"/>.
    /// </summary>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TestReplaceTE(bool removeSymbol)
        => TestReplaceSymbol(Terminal.Get("a"), EmptyString.Instance, removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing a <see cref="Nonterminal"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace the original with.</param>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow("a", "a", true)]
    [DataRow("a", "b", true)]
    [DataRow("a", "a", false)]
    [DataRow("a", "b", false)]
    public void TestReplaceNTNT(string symbol, string newSymbol, bool removeSymbol)
        => TestReplaceSymbol(Nonterminal.Get(symbol), Nonterminal.Get(newSymbol), removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing a <see cref="Nonterminal"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace the original with.</param>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow("a", "a", true)]
    [DataRow("a", "b", true)]
    [DataRow("a", "a", false)]
    [DataRow("a", "b", false)]
    public void TestReplaceNTT(string symbol, string newSymbol, bool removeSymbol)
        => TestReplaceSymbol(Nonterminal.Get(symbol), Terminal.Get(newSymbol), removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing a <see cref="Nonterminal"/> with an <see cref="EmptyString"/>.
    /// </summary>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TestReplaceNTE(bool removeSymbol)
        => TestReplaceSymbol(Nonterminal.Get("a"), EmptyString.Instance, removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing an <see cref="EmptyString"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TestReplaceET(bool removeSymbol)
        => TestReplaceSymbol(EmptyString.Instance, Terminal.Get("a"), removeSymbol);

    /// <summary>
    /// Tests the <see cref="CFG.ReplaceSymbol(ISymbol, ISymbol, bool)"/> function for replacing an <see cref="EmptyString"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="removeSymbol"><see langword="true"/> if the original symbol should be removed; <see langword="false"/> otherwise.</param>
    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TestReplaceENT(bool removeSymbol)
        => TestReplaceSymbol(EmptyString.Instance, Nonterminal.Get("a"), removeSymbol);

    #endregion ReplaceSymbol

    /// <summary>
    /// Tests the <see cref="CFG.ToString"/> function for a non-empty grammar.
    /// </summary>
    [TestMethod]
    public void TestToStringNotEmpty()
    {
        CFG grammar = new();

        Rule rule1 = grammar.AddRule("S");
        rule1.Productions.Add(new() { Nonterminal.Get("B"), Terminal.Get("c") });
        rule1.Productions.Add(new() { EmptyString.Instance });

        Rule rule2 = grammar.AddRule("B");
        rule2.Productions.Add(new() { Terminal.Get("b") });

        Assert.AreEqual($"S = B 'c' | '' ;{Environment.NewLine}B = 'b' ;", grammar.ToString());
    }
}
