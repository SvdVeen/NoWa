namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Terminal"/> class.
/// </summary>
[TestClass]
public class TerminalTests
{
    /// <summary>
    /// Tests whether the <see cref="Terminal.Get(string)"/> method properly instantiates <see cref="Terminal.Value"/>.
    /// </summary>
    [TestMethod]
    public void GetTest()
    {
        Terminal terminal = Terminal.Get("test");
        Assert.AreEqual("test", terminal.Value);
    }

    /// <summary>
    /// Tests whether the <see cref="Terminal.Get(string)"/> method throws an <see cref="ArgumentNullException"/> if an empty value is passed.
    /// </summary>
    [TestMethod]
    public void GetTestEmptyValue()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => _ = Terminal.Get(""));
    }

    /// <summary>
    /// Tests whether the <see cref="Terminal.Get(string)"/> method returns the same instance for the same value.
    /// </summary>
    [TestMethod]
    public void GetTestSame()
    {
        Terminal a = Terminal.Get("test");
        Terminal b = Terminal.Get("test");
        Assert.AreSame(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.ToString"/> function.
    /// </summary>
    [TestMethod]
    public void ToStringTest()
    {
        Terminal Terminal = Terminal.Get("test");
        Assert.AreEqual("'test'", Terminal.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Terminal.Equals"/> function for two equal terminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestEqual()
    {
        Terminal a = Terminal.Get("test");
        Terminal b = Terminal.Get("test");
        Assert.AreEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.Equals"/> function for two non-equal terminals.
    /// </summary>
    [TestMethod]
    public void EqualsTestNotEqual()
    {
        Terminal a = Terminal.Get("test");
        Terminal b = Terminal.Get("nope");
        Assert.AreNotEqual(a, b);
    }

    /// <summary>
    /// Tests the <see cref="Terminal.GetHashCode"/> function for two equal terminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        Terminal a = Terminal.Get("test");
        Terminal b = Terminal.Get("test");
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Terminal.GetHashCode"/> function for two non-equal terminals.
    /// </summary>
    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        Terminal a = Terminal.Get("test");
        Terminal b = Terminal.Get("nope");
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
