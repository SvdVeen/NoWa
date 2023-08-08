namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Rule"/> class.
/// </summary>
[TestClass]
[TestCategory($"{nameof(NoWa)}.{nameof(Common)}")]
public class RuleTests
{
    /// <summary>
    /// Tests the default constructor.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestConstructor)}")]
    public void TestConstructor()
    {
        Nonterminal a = new("a");
        Rule rule = new(a);
        Assert.AreSame(a, rule.Nonterminal, "Nonterminal did not match.");
        Assert.AreEqual(0, rule.Expressions.Count, "Expressions is not empty.");
    }

    /// <summary>
    /// Tests the getter and setter of <see cref="Rule.Nonterminal"/>.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestNonterminal)}")]
    public void TestNonterminal()
    {
        Rule rule = new(new("a"));
        Nonterminal b = new("b");
        rule.Nonterminal = b;
        Assert.AreSame(b, rule.Nonterminal);
    }

    /// <summary>
    /// Tests the <see cref="Rule.AddExpression"/> function.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestAddExpression)}")]
    public void TestAddExpression()
    {
        Rule rule = new(new("a"));
        Nonterminal b = new("b");
        Nonterminal c = new("c");
        rule.AddExpression(b, c);
        Assert.AreSame(b, rule.Expressions[0][0]);
        Assert.AreSame(c, rule.Expressions[0][1]);
    }

    /// <summary>
    /// Tests the <see cref="Expression.ToString"/> function for an empty rule.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestToStringEmpty)}")]
    public void TestToStringEmpty()
    {
        Rule rule = new(new("a"));
        Assert.AreEqual("Empty rule a", rule.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Expression.ToString"/> function for a non-empty rule.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestToStringNotEmpty)}")]
    public void TestToStringNotEmpty()
    {
        Rule rule = new(new("a"));
        rule.AddExpression(new Nonterminal("b"), new Terminal("a"));
        rule.AddExpression(new Terminal("c"));
        Assert.AreEqual("a = b 'a' | 'c' ;", rule.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Rule.Equals"/> for two equal instances.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestEqualsEqual)}")]
    public void TestEqualsEqual()
    {
        Rule a = new(new("test"));
        Rule b = new(new("test"));
        Assert.IsTrue(a.Equals(b));
    }

    /// <summary>
    /// Tests the <see cref="Rule.Equals"/> for two non-equal instances.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestEqualsNotEqual)}")]
    public void TestEqualsNotEqual()
    {
        Rule a = new(new("a"));
        Rule b = new(new("b"));
        Assert.IsFalse(a.Equals(b));
    }
    /// <summary>
    /// Tests the <see cref="Rule.GetHashCode"/> for two equal instances.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestGetHashCodeEqual)}")]
    public void TestGetHashCodeEqual()
    {
        Rule a = new(new("test"));
        Rule b = new(new("test"));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Rule.GetHashCode"/> for two non-equal instances.
    /// </summary>
    [TestMethod($"{nameof(Rule)}.{nameof(TestGetHashCodeNotEqual)}")]
    public void TestGetHashCodeNotEqual()
    {
        Rule a = new(new("a"));
        Rule b = new(new("b"));
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}