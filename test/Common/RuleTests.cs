namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Rule"/> class.
/// </summary>
[TestClass]
public class RuleTests
{
    /// <summary>
    /// Tests the default constructor.
    /// </summary>
    [TestMethod]
    public void TestConstructor()
    {
        Nonterminal a = Nonterminal.Get("a");
        Rule rule = new(a);
        Assert.AreSame(a, rule.Nonterminal, "Nonterminal did not match.");
        Assert.AreEqual(0, rule.Productions.Count, "Expressions is not empty.");
    }

    /// <summary>
    /// Tests the getter and setter of <see cref="Rule.Nonterminal"/>.
    /// </summary>
    [TestMethod]
    public void TestNonterminal()
    {
        Rule rule = new(Nonterminal.Get("a"));
        Nonterminal b = Nonterminal.Get("b");
        rule.Nonterminal = b;
        Assert.AreSame(b, rule.Nonterminal);
    }

    /// <summary>
    /// Tests the <see cref="Rule.AddProduction"/> function.
    /// </summary>
    [TestMethod]
    public void TestAddProduction()
    {
        Rule rule = new(Nonterminal.Get("a"));
        Nonterminal b = Nonterminal.Get("b");
        Nonterminal c = Nonterminal.Get("c");
        rule.AddProduction(b, c);
        Assert.AreSame(b, rule.Productions[0][0]);
        Assert.AreSame(c, rule.Productions[0][1]);
    }

    /// <summary>
    /// Helper for testing the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function.
    /// </summary>
    /// <param name="symbol">The symbol to replace.</param>
    /// <param name="newSymbol">The symbol to replace the original with.</param>
    private static void TestReplaceSymbol(ISymbol symbol, ISymbol newSymbol)
    {
        Nonterminal Ntest = Nonterminal.Get("test");
        Terminal Ttest = Terminal.Get("test");
        Rule rule = new(Ntest);
        rule.Productions.Add(new() { symbol, Ntest, Ntest, symbol, Ttest });
        rule.Productions.Add(new() { Ttest, Ntest, Ttest });
        rule.Productions.Add(new() { Ttest, symbol, symbol });
        rule.ReplaceSymbol(symbol, newSymbol);
        Assert.AreSame(Ntest, rule.Nonterminal, "The rule's nonterminal does not match the original.");
        ExpressionAssert.AreSameEntries(rule.Productions[0], newSymbol, Ntest, Ntest, newSymbol, Ttest);
        ExpressionAssert.AreSameEntries(rule.Productions[1], Ttest, Ntest, Ttest);
        ExpressionAssert.AreSameEntries(rule.Productions[2], Ttest, newSymbol, newSymbol);
    }

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace it with.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceTT(string symbol, string newSymbol) => TestReplaceSymbol(Terminal.Get(symbol), Terminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace it with.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceTNT(string symbol, string newSymbol) => TestReplaceSymbol(Terminal.Get(symbol), Nonterminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with an <see cref="EmptyString"/>.
    /// </summary>
    [TestMethod]
    public void TestReplaceTE() => TestReplaceSymbol(Terminal.Get("a"), EmptyString.Instance);

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing a <see cref="Nonterminal"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace it with.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceNTNT(string symbol, string newSymbol) => TestReplaceSymbol(Nonterminal.Get(symbol), Nonterminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing a <see cref="Nonterminal"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the symbol to replace.</param>
    /// <param name="newSymbol">The value of the symbol to replace it with.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceNTT(string symbol, string newSymbol) => TestReplaceSymbol(Nonterminal.Get(symbol), Terminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with an <see cref="EmptyString"/>.
    /// </summary>
    [TestMethod]
    public void TestReplaceNTE() => TestReplaceSymbol(Nonterminal.Get("a"), EmptyString.Instance);

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing an <see cref="EmptyString"/> with a <see cref="Terminal"/>
    /// </summary>
    [TestMethod]
    public void TestReplaceET() => TestReplaceSymbol(EmptyString.Instance, Terminal.Get("a"));

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing an <see cref="EmptyString"/> with a <see cref="Nonterminal"/>
    /// </summary>
    [TestMethod]
    public void TestReplaceENT() => TestReplaceSymbol(EmptyString.Instance, Nonterminal.Get("a"));

    /// <summary>
    /// Tests the <see cref="Rule.ReplaceSymbol(ISymbol, ISymbol)"/> function for replacing its nonterminal.
    /// </summary>
    [TestMethod]
    public void TestReplaceRuleNonterminal()
    {
        Nonterminal a = Nonterminal.Get("a");
        Nonterminal b = Nonterminal.Get("b");
        Nonterminal test = Nonterminal.Get("test");
        Rule rule = new(a);
        rule.Productions.Add(new() { test });
        rule.ReplaceSymbol(a, b);
        Assert.AreSame(b, rule.Nonterminal);
        Assert.AreSame(test, rule.Productions[0][0]);
    }

    /// <summary>
    /// Tests the <see cref="Expression.ToString"/> function for an empty rule.
    /// </summary>
    [TestMethod]
    public void TestToStringEmpty()
    {
        Rule rule = new(Nonterminal.Get("a"));
        Assert.AreEqual("Empty rule a", rule.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Expression.ToString"/> function for a non-empty rule.
    /// </summary>
    [TestMethod]
    public void TestToStringNotEmpty()
    {
        Rule rule = new(Nonterminal.Get("a"));
        rule.AddProduction(Nonterminal.Get("b"), Terminal.Get("a"));
        rule.AddProduction(Terminal.Get("c"));
        Assert.AreEqual("a = b 'a' | 'c' ;", rule.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Rule.Equals"/> for two equal instances.
    /// </summary>
    [TestMethod]
    public void TestEqualsEqual()
    {
        Rule a = new(Nonterminal.Get("test"));
        Rule b = new(Nonterminal.Get("test"));
        Assert.IsTrue(a.Equals(b));
    }

    /// <summary>
    /// Tests the <see cref="Rule.Equals"/> for two non-equal instances.
    /// </summary>
    [TestMethod]
    public void TestEqualsNotEqual()
    {
        Rule a = new(Nonterminal.Get("a"));
        Rule b = new(Nonterminal.Get("b"));
        Assert.IsFalse(a.Equals(b));
    }
    /// <summary>
    /// Tests the <see cref="Rule.GetHashCode"/> for two equal instances.
    /// </summary>
    [TestMethod]
    public void TestGetHashCodeEqual()
    {
        Rule a = new(Nonterminal.Get("test"));
        Rule b = new(Nonterminal.Get("test"));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Rule.GetHashCode"/> for two non-equal instances.
    /// </summary>
    [TestMethod]
    public void TestGetHashCodeNotEqual()
    {
        Rule a = new(Nonterminal.Get("a"));
        Rule b = new(Nonterminal.Get("b"));
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
