namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Terminal"/> class.
/// </summary>
[TestClass]
[TestCategory($"{nameof(NoWa)}.{nameof(Common)}")]
public class TerminalTests
{
    /// <summary>
    /// Tests whether the constructor properly instantiates <see cref="Terminal.Value"/>.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ConstructorTest)}")]
    public void ConstructorTest()
    {
        Terminal Terminal = new("test");
        Assert.AreEqual("test", Terminal.Value);
    }

    /// <summary>
    /// Tests whether the constructor throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ConstructorTestEmptyValue)}")]
    public void ConstructorTestEmptyValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Terminal(""));
    }

    /// <summary>
    /// Tests whether the constructor throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ConstructorTestWhiteSpaceValue)}")]
    public void ConstructorTestWhiteSpaceValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Terminal("    "));
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Terminal.Value"/> and its getter work properly.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ValueTest)}")]
    public void ValueTest()
    {
#pragma warning disable IDE0017 // Simplify object initialization
        Terminal Terminal = new("a");
#pragma warning restore IDE0017 // Simplify object initialization
        Terminal.Value = "b";
        Assert.AreEqual("b", Terminal.Value);
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Terminal.Value"/> throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ValueTestEmpty)}")]
    public void ValueTestEmpty()
    {
        Terminal Terminal = new("test");
        _ = Assert.ThrowsException<ArgumentNullException>(() => Terminal.Value = "");
    }

    /// <summary>
    /// Tests whether the setter of <see cref="Terminal.Value"/> throws an <see cref="ArgumentNullException"/> if a whitespace value is passed.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ValueTestWhiteSpace)}")]
    public void ValueTestWhiteSpace()
    {
        Terminal Terminal = new("test");
        _ = Assert.ThrowsException<ArgumentNullException>(() => Terminal.Value = "     ");
    }

    /// <summary>
    /// Tests the <see cref="Terminal.ToString"/> function.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(ToStringTest)}")]
    public void ToStringTest()
    {
        Terminal Terminal = new("test");
        Assert.AreEqual("'test'", Terminal.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Terminal.Equals"/> function for two equal terminals.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(EqualsTestEqual)}")]
    public void EqualsTestEqual()
    {
        Terminal a = new("test");
        Terminal b = new("test");
        Assert.AreEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.Equals"/> function for two non-equal terminals.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(EqualsTestNotEqual)}")]
    public void EqualsTestNotEqual()
    {
        Terminal a = new("test");
        Terminal b = new("nope");
        Assert.AreNotEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.GetHashCode"/> function for two equal terminals.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(GetHashCodeTestEqual)}")]
    public void GetHashCodeTestEqual()
    {
        Terminal a = new("test");
        Terminal b = new("test");
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Terminal.GetHashCode"/> function for two non-equal terminals.
    /// </summary>
    [TestMethod($"{nameof(Terminal)}.{nameof(GetHashCodeTestNotEqual)}")]
    public void GetHashCodeTestNotEqual()
    {
        Terminal a = new("test");
        Terminal b = new("nope");
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}