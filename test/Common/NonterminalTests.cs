namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Nonterminal"/> class.
/// </summary>
[TestClass]
public class NonterminalTests
{
    /// <summary>
    /// Tests whether the <see cref="Nonterminal.Get(string)"/> method properly instantiates <see cref="Nonterminal.Value"/>.
    /// </summary>
    [TestMethod]
    public void GetTest()
    {
        Nonterminal nonterminal = Nonterminal.Get("test");
        Assert.AreEqual("test", nonterminal.Value);
    }

    /// <summary>
    /// Tests whether the <see cref="Nonterminal.Get(string)"/> method throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod]
    public void GetTestEmptyValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = Nonterminal.Get(""));
    }

    /// <summary>
    /// Tests whether the <see cref="Nonterminal.Get(string)"/> method throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod]
    public void GetTestWhiteSpaceValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = Nonterminal.Get("    "));
    }

    /// <summary>
    /// Tests whether the <see cref="Nonterminal.Get(string)"/> method returns the same instance for the same value.
    /// </summary>
    [TestMethod]
    public void GetTestSame()
    {
        Nonterminal a = Nonterminal.Get("test");
        Nonterminal b = Nonterminal.Get("test");
        Assert.AreSame(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.ToString"/> function.
    /// </summary>
    [TestMethod]
    public void ToStringTest()
    {
        Nonterminal nonterminal = Nonterminal.Get("test");
        Assert.AreEqual("test", nonterminal.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.Equals"/> function for two equal nonterminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestEqual()
    {
        Nonterminal a = Nonterminal.Get("test");
        Nonterminal b = Nonterminal.Get("test");
        Assert.AreEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.Equals"/> function for two non-equal nonterminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestNotEqual()
    {
        Nonterminal a = Nonterminal.Get("test");
        Nonterminal b = Nonterminal.Get("nope");
        Assert.AreNotEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.GetHashCode"/> function for two equal nonterminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        Nonterminal a = Nonterminal.Get("test");
        Nonterminal b = Nonterminal.Get("test");
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Nonterminal.GetHashCode"/> function for two non-equal nonterminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        Nonterminal a = Nonterminal.Get("test");
        Nonterminal b = Nonterminal.Get("nope");
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
