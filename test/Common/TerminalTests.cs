namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Terminal"/> class.
/// </summary>
[TestClass]
public class TerminalTests
{
    /// <summary>
    /// Tests whether the constructor properly instantiates <see cref="Terminal.Value"/>.
    /// </summary>
    [TestMethod]
    public void ConstructorTest()
    {
        Terminal terminal = new("test");
        Assert.AreEqual("test", terminal.Value);
    }

    /// <summary>
    /// Tests whether the constructor throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod]
    public void ConstructorTestEmptyValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Terminal(""));
    }

    /// <summary>
    /// Tests whether the constructor throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod]
    public void ConstructorTestWhiteSpaceValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Terminal("    "));
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Terminal.Value"/> and its getter work properly.
    /// </summary>
    [TestMethod]
    public void ValueTest()
    {
#pragma warning disable IDE0017 // Simplify object initialization
        Terminal terminal = new("a");
#pragma warning restore IDE0017 // Simplify object initialization
        terminal.Value = "b";
        Assert.AreEqual("b", terminal.Value);
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Terminal.Value"/> throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod]
    public void ValueTestEmpty()
    {
        Terminal terminal = new("test");
        _ = Assert.ThrowsException<ArgumentNullException>(() => terminal.Value = "");
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Terminal.Value"/> throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod]
    public void ValueTestWhiteSpace()
    {
        Terminal terminal = new("test");
        _ = Assert.ThrowsException<ArgumentNullException>(() => terminal.Value = "     ");
    }

    /// <summary>
    /// Tests the <see cref="Terminal.ToString"/> function.
    /// </summary>
    [TestMethod]
    public void ToStringTest()
    {
        Terminal Terminal = new("test");
        Assert.AreEqual("'test'", Terminal.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Terminal.Equals"/> function for two equal terminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestEqual()
    {
        Terminal a = new("test");
        Terminal b = new("test");
        Assert.AreEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.Equals"/> function for two non-equal terminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestNotEqual()
    {
        Terminal a = new("test");
        Terminal b = new("nope");
        Assert.AreNotEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.GetHashCode"/> function for two equal terminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        Terminal a = new("test");
        Terminal b = new("test");
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Terminal.GetHashCode"/> function for two non-equal terminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        Terminal a = new("test");
        Terminal b = new("nope");
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
