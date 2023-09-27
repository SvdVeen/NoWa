namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Production"/> class.
/// </summary>
[TestClass]
public class ProductionTests
{
    /// <summary>
    /// Tests the <see cref="Production(Nonterminal, Expression)"/> constructor.
    /// </summary>
    [TestMethod]
    public void TestProduction1()
    {
        Production production = new(Nonterminal.Get("A"), new Expression()
        {
            Nonterminal.Get("B"),
            Terminal.Get("a")
        });
        Assert.AreSame(Nonterminal.Get("A"), production.Head);
        Assert.AreEqual(2, production.Body.Count);
        Assert.AreSame(Nonterminal.Get("B"), production.Body[0]);
        Assert.AreSame(Terminal.Get("a"), production.Body[1]);
    }

    /// <summary>
    /// Tests the <see cref="Production(Nonterminal, IEnumerable{ISymbol})"/> constructor.
    /// </summary>
    [TestMethod]
    public void TestProduction2()
    {
        Production production = new(Nonterminal.Get("A"), new List<ISymbol> {
            Nonterminal.Get("B"),
            Terminal.Get("a")
        });
        Assert.AreSame(Nonterminal.Get("A"), production.Head);
        Assert.AreEqual(2, production.Body.Count);
        Assert.AreSame(Nonterminal.Get("B"), production.Body[0]);
        Assert.AreSame(Terminal.Get("a"), production.Body[1]);
    }

    /// <summary>
    /// Tests the <see cref="Production(Nonterminal, ISymbol[])"/> constructor.
    /// </summary>
    [TestMethod]
    public void TestProduction3()
    {
        Production production = new(Nonterminal.Get("A"), Nonterminal.Get("B"), Terminal.Get("a"));
        Assert.AreSame(Nonterminal.Get("A"), production.Head);
        Assert.AreEqual(2, production.Body.Count);
        Assert.AreSame(Nonterminal.Get("B"), production.Body[0]);
        Assert.AreSame(Terminal.Get("a"), production.Body[1]);
    }

    /// <summary>
    /// Tests the <see cref="Production.ToString"/> method for a non-empty production.
    /// </summary>
    [TestMethod]
    public void TestToString()
    {
        Production production = new(Nonterminal.Get("A"), Terminal.Get("b"), Terminal.Get("c"));
        Assert.AreEqual("A --> 'b' 'c' ;", production.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Production.ToString"/> method for an empty production.
    /// </summary>
    [TestMethod]
    public void TestToStringEmpty()
    {
        Production production = new(Nonterminal.Get("A"));
        Assert.AreEqual("Empty production A ;", production.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Production.Equals(object?)"/> method for two equal productions.
    /// </summary>
    [TestMethod]
    public void TestEqualsEqual()
    {
        Production a = new Production(Nonterminal.Get("A"), Terminal.Get("a"));
        Production b = new Production(Nonterminal.Get("A"), Terminal.Get("a"));
        Assert.AreEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Production.Equals(object?)"/> method for two non-equal productions.
    /// </summary>
    [TestMethod]
    public void TestEqualsNotEqual()
    {
        Production a = new Production(Nonterminal.Get("A"), Terminal.Get("a"));
        Production b = new Production(Nonterminal.Get("B"), Terminal.Get("b"));
        Assert.AreNotEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Production.GetHashCode"/> method for two non-equal productions.
    /// </summary>
    [TestMethod]
    public void TestGetHashCodeEqual()
    {
        Production a = new Production(Nonterminal.Get("A"), Terminal.Get("a"));
        Production b = new Production(Nonterminal.Get("A"), Terminal.Get("a"));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Production.GetHashCode"/> method for two non-equal productions.
    /// </summary>
    [TestMethod]
    public void TestGetHashCodeNotEqual()
    {
        Production a = new Production(Nonterminal.Get("A"), Terminal.Get("a"));
        Production b = new Production(Nonterminal.Get("B"), Terminal.Get("b"));
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}