namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Nonterminal"/> class.
/// </summary>
[TestClass]
public class NonterminalTests
{
    /// <summary>
    /// Tests whether the constructor properly instantiates <see cref="Nonterminal.Value"/>.
    /// </summary>
    [TestMethod]
    public void ConstructorTest()
    {
        Nonterminal nonterminal = new("test");
        Assert.AreEqual("test", nonterminal.Value);
    }

    /// <summary>
    /// Tests whether the constructor throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod]
    public void ConstructorTestEmptyValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Nonterminal(""));
    }

    /// <summary>
    /// Tests whether the constructor throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod]
    public void ConstructorTestWhiteSpaceValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Nonterminal("    "));
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Nonterminal.Value"/> and its getter work properly.
    /// </summary>
    [TestMethod]
    public void ValueTest()
    {
#pragma warning disable IDE0017 // Simplify object initialization
        Nonterminal nonterminal = new("a");
#pragma warning restore IDE0017 // Simplify object initialization
        nonterminal.Value = "b";
        Assert.AreEqual("b", nonterminal.Value);
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Nonterminal.Value"/> throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod]
    public void ValueTestEmpty()
    {
        Nonterminal nonterminal = new("test");
        _ = Assert.ThrowsException<ArgumentNullException>(() => nonterminal.Value = "");
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Nonterminal.Value"/> throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod]
    public void ValueTestWhiteSpace()
    {
        Nonterminal nonterminal = new("test");
        _ = Assert.ThrowsException<ArgumentNullException>(() => nonterminal.Value = "     ");
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.ToString"/> function.
    /// </summary>
    [TestMethod]
    public void ToStringTest()
    {
        Nonterminal nonterminal = new("test");
        Assert.AreEqual("test", nonterminal.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.Equals"/> function for two equal nonterminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestEqual()
    {
        Nonterminal a = new("test");
        Nonterminal b = new("test");
        Assert.AreEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.Equals"/> function for two non-equal nonterminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestNotEqual()
    {
        Nonterminal a = new("test");
        Nonterminal b = new("nope");
        Assert.AreNotEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.GetHashCode"/> function for two equal nonterminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        Nonterminal a = new("test");
        Nonterminal b = new("test");
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.GetHashCode"/> function for two non-equal nonterminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        Nonterminal a = new("test");
        Nonterminal b = new("nope");
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
