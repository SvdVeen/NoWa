namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="EmptyString"/> class.
/// </summary>
[TestClass]
public class EmptyStringTests
{
    /// <summary>
    /// Test the <see cref="EmptyString.ToString"/> function.
    /// </summary>
    [TestMethod]
    public void ToStringTest()
    {
        Assert.AreEqual("''", EmptyString.Instance.ToString());
    }

    /// <summary>
    /// Test the <see cref="EmptyString.Equals"/> function.
    /// </summary>
    [TestMethod]
    public void EqualsTest()
    {
        Assert.IsTrue(EmptyString.Instance.Equals(EmptyString.Instance), "The instance does not equal itself");
        Assert.IsFalse(EmptyString.Instance.Equals("''"), "The instance equals something other than itself");
    }

    /// <summary>
    /// Tests the <see cref="EmptyString.GetHashCode"/> function.
    /// </summary>

    [TestMethod]
    public void GetHashCodeTest()
    {
        Assert.AreEqual(EmptyString.Instance.GetHashCode(), EmptyString.Instance.GetHashCode(), "Two hashcodes are different");
    }
}
