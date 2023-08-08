namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="Expression"/> class.
/// </summary>
/// <remarks>
/// The unit tests for <see cref="IList{T}"/> implementations are not exactly ironclad, but I do not plan to change them,
/// and I trust that it being proxied to an internal list will be fine for the near future.
/// </remarks>
[TestClass]
[TestCategory($"{nameof(NoWa)}.{nameof(Common)}")]
public class ExpressionTests
{
    /// <summary>
    /// Tests the constructor of an empty <see cref="Expression"/>.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestEmpty)}")]
    public void TestEmpty()
    {
        Expression expression = new();
        Assert.AreEqual(0, expression.Count, "The count of an empty expression is not zero.");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = expression[0], "The index accessor of an empty expression did not throw an exception.");
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => expression[15] = EmptyString.Instance, "The index setter of an empty expression did not throw an exception.");
        Assert.AreEqual("Empty expression", expression.ToString(), "Empty ToString message does not match expected message.");
    }

    /// <summary>
    /// Tests the constructor of an expression with a pre-existing set of symbols.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestPrefilled)}")]
    public void TestPrefilled()
    {
        Nonterminal a = new("a");
        Terminal b = new("b");
        Expression expression = new(new ISymbol[] { a, EmptyString.Instance, b });
        Assert.AreEqual(3, expression.Count, "The count of a pre-filled expression does not match.");
        Assert.AreSame(a, expression[0], "The index accessor of a pre-filled expression does not match.");
        Assert.AreEqual("a '' 'b'", expression.ToString(), "The ToString message for a pre-filled expression does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Expression.Add"/> function of an expression.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestAdd)}")]
    public void TestAdd()
    {
        Expression expression = new();
        Nonterminal a = new("a");
        expression.Add(a);
        Assert.AreSame(a, expression[0]);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Clear"/> function. 
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestAdd)}")]
    public void TestClear()
    {
        Expression expression = new() { EmptyString.Instance };
        expression.Clear();
        Assert.AreEqual(0, expression.Count);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Contains"/> function.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestContains)}")]
    public void TestContains()
    {
        Nonterminal a = new("a");
        Expression expression = new() { EmptyString.Instance, a };
        Assert.IsTrue(expression.Contains(a));
    }

    /// <summary>
    /// Tests the <see cref="Expression.CopyTo"/> function.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestCopyTo)}")]
    public void TestCopyTo()
    {
        Nonterminal a = new("a");
        Terminal b = new("b");
        Expression expression = new() { a, b };
        ISymbol[] array = new ISymbol[2];
        expression.CopyTo(array, 0);
        Assert.AreSame(expression[0], array[0]);
        Assert.AreSame(expression[1], array[1]);
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.GetEnumerator"/> gives a correct enumerator.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestEnumerator)}")]
    public void TestEnumerator()
    {
        Expression expression = new()
        {
            new Nonterminal("a"),
            new Nonterminal("b"),
            new Terminal("c"),
            EmptyString.Instance
        };

        int i = 0;
        foreach (ISymbol symbol in expression)
        {
            Assert.AreSame(expression[i], symbol, $"Symbol at index {i++} does not match enumerator.");
        }
    }

    /// <summary>
    /// Tests the <see cref="Expression.IndexOf"/> function.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestIndexOf)}")]
    public void TestIndexOf()
    {
        Nonterminal a = new("a");
        Expression expression = new() { new Nonterminal("n"), a, EmptyString.Instance };
        Assert.AreEqual(1, expression.IndexOf(a));
    }

    /// <summary>
    /// Tests the <see cref="Expression.Insert"/> function.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestInsert)}")]
    public void TestInsert()
    {
        Nonterminal a = new("a");
        Expression expression = new() { EmptyString.Instance, EmptyString.Instance, new Terminal("b") };
        expression.Insert(1, a);
        Assert.AreSame(a, expression[1]);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Remove"/> function.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestRemove)}")]
    public void TestRemove()
    {
        Terminal b = new("b");
        Expression expression = new() { new Nonterminal("c"), b, EmptyString.Instance };
        expression.Remove(b);
        Assert.AreEqual(2, expression.Count, "Count did not match");
        Assert.IsFalse(expression.Contains(b), "Expression contains removed item.");
    }

    /// <summary>
    /// Tests the <see cref="Expression.RemoveAt"/> function.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestRemoveAt)}")]
    public void TestRemoveAt()
    {
        Expression expression = new() { new Nonterminal("b"), new Terminal("c"), EmptyString.Instance };
        expression.RemoveAt(1);
        Assert.AreEqual(2, expression.Count);
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.Equals"/> function returns <see langword="true"/> for two equal instances.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestEqualsEqual)}")]
    public void TestEqualsEqual()
    {
        Expression a = new() { new Nonterminal("a"), new Terminal("b"), new Nonterminal("c") };
        Expression b = new(a);
        Assert.IsTrue(a.Equals(b));
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.Equals"/> function returns <see langword="false"/> for two non-equal instances.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestEqualsNotEqual)}")]
    public void TestEqualsNotEqual()
    {
        Expression a = new() { new Nonterminal("a"), new Terminal("b"), new Nonterminal("c") };
        Expression b = new() { new Terminal("q"), new Nonterminal("f"), EmptyString.Instance };
        Assert.IsFalse(a.Equals(b));
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.GetHashCode"/> function returns equal hashes for two equal instances.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestHashCodeEqual)}")]
    public void TestHashCodeEqual()
    {
        Expression a = new() { new Nonterminal("a"), new Terminal("b"), new Nonterminal("c") };
        Expression b = new(a);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.GetHashCode"/> function returns different hashes for two non-equal instances.
    /// </summary>
    [TestMethod($"{nameof(Expression)}.{nameof(TestHashCodeNotEqual)}")]
    public void TestHashCodeNotEqual()
    {
        Expression a = new() { new Nonterminal("a"), new Terminal("b"), new Nonterminal("c") };
        Expression b = new() { new Terminal("q"), new Nonterminal("f"), EmptyString.Instance };
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}