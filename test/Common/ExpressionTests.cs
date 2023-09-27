namespace NoWa.Common.Tests;

/// <summary>
/// Helper class with functions for comparing the entries of <see cref="Expression"/> instances.
/// </summary>
internal static class ExpressionAssert
{
    /// <summary>
    /// Tests whether the entries of two expressions are the same.
    /// </summary>
    /// <param name="expected">The expected expression.</param>
    /// <param name="actual">The actual expression.</param>
    /// <param name="message">The message to add to the exception if the assertions fail.</param>
    /// <exception cref="AssertFailedException">The two expressions do not have the same entries.</exception>
    private static void AreSameEntriesImp(Expression expected, Expression actual, string? message)
    {
        try
        {
            Assert.AreEqual(actual.Count, expected.Count, "The actual expression is longer than the expected expression.");
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreSame(expected[i], actual[i], $"The symbol at index {i} did not match the expected symbol.");
            }
        }
        catch (AssertFailedException ex)
        {
            message ??= $"The expression does not match the expected expression: {ex.Message}";
            throw new AssertFailedException(message, ex);
        }
    }

    /// <summary>
    /// Tests whether the entries of two expressions are the same.
    /// </summary>
    /// <param name="expected">The expected expression.</param>
    /// <param name="actual">The actual expression.</param>
    /// <exception cref="AssertFailedException">The two expressions do not have the same entries.</exception>
    internal static void AreSameEntries(Expression expected, Expression actual)
        => AreSameEntriesImp(expected, actual, null);

    internal static void AreSameEntries(Expression expected, Expression actual, string message)
        => AreSameEntriesImp(expected, actual, message);

    /// <summary>
    /// Tests whether an expression matches a sequence of expected symbols and throws an exception if they do not contain the same items.
    /// </summary>
    /// <param name="actual">The actual expression.</param>
    /// <param name="message">The message to add to the exception if the assertions fail.</param>
    /// <param name="expected">The expected sequence of symbols.</param>
    /// <exception cref="AssertFailedException">The expression's entries are not the same as the expected symbols.</exception>
    private static void AreSameEntriesImp(Expression actual, string? message, params ISymbol[] expected)
    {
        try
        {
            Assert.AreEqual(actual.Count, expected.Length, "The actual expression is longer than the number of expected symbols.");
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreSame(expected[i], actual[i], $"The symbol at index {i} did not match the expected symbol.");
            }
        }
        catch (AssertFailedException ex)
        {
            message ??= $"The expression does not match the expected symbols: {ex.Message}";
            throw new AssertFailedException(message, ex);
        }
    }

    /// <summary>
    /// Tests whether the entries of an expression match expected symbols.
    /// </summary>
    /// <param name="actual">The actual expression.</param>
    /// <param name="expected">The expected symbols.</param>
    /// <exception cref="AssertFailedException">The expression's symbols do not match the expected symbols.</exception>
    internal static void AreSameEntries(Expression actual, params ISymbol[] expected)
        => AreSameEntriesImp(actual, null, expected);

    /// <summary>
    /// Tests whether the entries of an expression match expected symbols.
    /// </summary>
    /// <param name="actual">The actual expression.</param>
    /// <param name="message">The message to add to the exception if the assertions fail.</param>
    /// <param name="expected">The expected symbols.</param>
    /// <exception cref="AssertFailedException">The expression's symbols do not match the expected symbols.</exception>
    internal static void AreSameEntries(Expression actual, string message, params ISymbol[] expected)
        => AreSameEntriesImp(actual, message, expected);
}

/// <summary>
/// Contains unit tests for the <see cref="Expression"/> class.
/// </summary>
/// <remarks>
/// The unit tests for <see cref="IList{T}"/> implementations are not exactly ironclad, but I do not plan to change them,
/// and I trust that it being proxied to an internal list will be fine for the near future.
/// </remarks>
[TestClass]
public class ExpressionTests
{
    /// <summary>
    /// Tests the constructor of an empty <see cref="Expression"/>.
    /// </summary>
    [TestMethod]
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
    [TestMethod]
    public void TestPrefilled()
    {
        Nonterminal a = Nonterminal.Get("a");
        Terminal b = Terminal.Get("b");
        Expression expression = new(new ISymbol[] { a, EmptyString.Instance, b });
        Assert.AreEqual(3, expression.Count, "The count of a pre-filled expression does not match.");
        Assert.AreSame(a, expression[0], "The index accessor of a pre-filled expression does not match.");
        Assert.AreEqual("a '' 'b'", expression.ToString(), "The ToString message for a pre-filled expression does not match.");
    }

    /// <summary>
    /// Tests the <see cref="Expression.Add"/> function of an expression.
    /// </summary>
    [TestMethod]
    public void TestAdd()
    {
        Expression expression = new();
        Nonterminal a = Nonterminal.Get("a");
        expression.Add(a);
        Assert.AreSame(a, expression[0]);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Clear"/> function. 
    /// </summary>
    [TestMethod]
    public void TestClear()
    {
        Expression expression = new() { EmptyString.Instance };
        expression.Clear();
        Assert.AreEqual(0, expression.Count);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Contains"/> function.
    /// </summary>
    [TestMethod]
    public void TestContains()
    {
        Nonterminal a = Nonterminal.Get("a");
        Expression expression = new() { EmptyString.Instance, a };
        Assert.IsTrue(expression.Contains(a));
    }

    /// <summary>
    /// Tests the <see cref="Expression.CopyTo"/> function.
    /// </summary>
    [TestMethod]
    public void TestCopyTo()
    {
        Nonterminal a = Nonterminal.Get("a");
        Terminal b = Terminal.Get("b");
        Expression expression = new() { a, b };
        ISymbol[] array = new ISymbol[2];
        expression.CopyTo(array, 0);
        Assert.AreSame(expression[0], array[0]);
        Assert.AreSame(expression[1], array[1]);
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.GetEnumerator"/> gives a correct enumerator.
    /// </summary>
    [TestMethod]
    public void TestEnumerator()
    {
        Expression expression = new()
        {
            Nonterminal.Get("a"),
            Nonterminal.Get("b"),
            Terminal.Get("c"),
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
    [TestMethod]
    public void TestIndexOf()
    {
        Nonterminal a = Nonterminal.Get("a");
        Expression expression = new() { Nonterminal.Get("n"), a, EmptyString.Instance };
        Assert.AreEqual(1, expression.IndexOf(a));
    }

    /// <summary>
    /// Tests the <see cref="Expression.Insert"/> function.
    /// </summary>
    [TestMethod]
    public void TestInsert()
    {
        Nonterminal a = Nonterminal.Get("a");
        Expression expression = new() { EmptyString.Instance, EmptyString.Instance, Terminal.Get("b") };
        expression.Insert(1, a);
        Assert.AreSame(a, expression[1]);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Remove"/> function.
    /// </summary>
    [TestMethod]
    public void TestRemove()
    {
        Terminal b = Terminal.Get("b");
        Expression expression = new() { Nonterminal.Get("c"), b, EmptyString.Instance };
        expression.Remove(b);
        Assert.AreEqual(2, expression.Count, "Count did not match");
        Assert.IsFalse(expression.Contains(b), "Expression contains removed item.");
    }

    /// <summary>
    /// Tests the <see cref="Expression.RemoveAt"/> function.
    /// </summary>
    [TestMethod]
    public void TestRemoveAt()
    {
        Expression expression = new() { Nonterminal.Get("b"), Terminal.Get("c"), EmptyString.Instance };
        expression.RemoveAt(1);
        Assert.AreEqual(2, expression.Count);
    }


    /// <summary>
    /// Helper for testing the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function.
    /// </summary>
    /// <param name="symbol">The symbol to replace.</param>
    /// <param name="newSymbol">The symbol to replace the original with.</param>
    private static void TestReplaceSymbol(ISymbol symbol, ISymbol newSymbol)
    {
        Nonterminal NTest = Nonterminal.Get("test");
        Terminal TTest = Terminal.Get("test");
        Expression expression = new() { NTest, symbol, NTest, NTest, symbol, symbol, TTest };
        expression.Replace(symbol, newSymbol);
        ExpressionAssert.AreSameEntries(expression, NTest, newSymbol, NTest, NTest, newSymbol, newSymbol, TTest);
    }

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the original symbol.</param>
    /// <param name="newSymbol">The value of the new symbol.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceTT(string symbol, string newSymbol) =>
        TestReplaceSymbol(Terminal.Get(symbol), Terminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the original symbol.</param>
    /// <param name="newSymbol">The value of the new symbol.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceTNT(string symbol, string newSymbol) =>
        TestReplaceSymbol(Terminal.Get(symbol), Nonterminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing a <see cref="Terminal"/> with an <see cref="EmptyString"/>.
    /// </summary>
    [TestMethod]
    public void TestReplaceTE() =>
        TestReplaceSymbol(Terminal.Get("a"), EmptyString.Instance);

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing a <see cref="Nonterminal"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the original symbol.</param>
    /// <param name="newSymbol">The value of the new symbol.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceNTNT(string symbol, string newSymbol) =>
        TestReplaceSymbol(Nonterminal.Get(symbol), Nonterminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing a <see cref="Nonterminal"/> with a <see cref="Terminal"/>.
    /// </summary>
    /// <param name="symbol">The value of the original symbol.</param>
    /// <param name="newSymbol">The value of the new symbol.</param>
    [DataTestMethod]
    [DataRow("a", "a")]
    [DataRow("a", "b")]
    public void TestReplaceNTT(string symbol, string newSymbol) =>
        TestReplaceSymbol(Nonterminal.Get(symbol), Terminal.Get(newSymbol));

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing a <see cref="Nonterminal"/> with an <see cref="EmptyString"/>.
    /// </summary>
    [TestMethod]
    public void TestReplaceNTE() =>
        TestReplaceSymbol(Nonterminal.Get("a"), EmptyString.Instance);

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing an <see cref="EmptyString"/> with a <see cref="Terminal"/>.
    /// </summary>
    [TestMethod]
    public void TestReplaceET() => TestReplaceSymbol(EmptyString.Instance, Terminal.Get("a"));

    /// <summary>
    /// Tests the <see cref="Expression.Replace(ISymbol, ISymbol)"/> function for replacing an <see cref="EmptyString"/> with a <see cref="Nonterminal"/>.
    /// </summary>
    [TestMethod]
    public void TestReplaceENT() => TestReplaceSymbol(EmptyString.Instance, Nonterminal.Get("a"));

    /// <summary>
    /// Tests the <see cref="Expression.ToString"/> function for a non-empty expression.
    /// </summary>
    [TestMethod]
    public void TestToStringNotEmpty()
    {
        Expression expression = new() { Nonterminal.Get("a"), Terminal.Get("b"), EmptyString.Instance };
        Assert.AreEqual("a 'b' ''", expression.ToString());
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.Equals"/> function returns <see langword="true"/> for two equal instances.
    /// </summary>
    [TestMethod]
    public void TestEqualsEqual()
    {
        Expression a = new() { Nonterminal.Get("a"), Terminal.Get("b"), Nonterminal.Get("c") };
        Expression b = new(a);
        Assert.IsTrue(a.Equals(b));
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.Equals"/> function returns <see langword="false"/> for two non-equal instances.
    /// </summary>
    [TestMethod]
    public void TestEqualsNotEqual()
    {
        Expression a = new() { Nonterminal.Get("a"), Terminal.Get("b"), Nonterminal.Get("c") };
        Expression b = new() { Terminal.Get("q"), Nonterminal.Get("f"), EmptyString.Instance };
        Assert.IsFalse(a.Equals(b));
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.GetHashCode"/> function returns equal hashes for two equal instances.
    /// </summary>
    [TestMethod]
    public void TestHashCodeEqual()
    {
        Expression a = new() { Nonterminal.Get("a"), Terminal.Get("b"), Nonterminal.Get("c") };
        Expression b = new(a);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    /// <summary>
    /// Tests whether the <see cref="Expression.GetHashCode"/> function returns different hashes for two non-equal instances.
    /// </summary>
    [TestMethod]
    public void TestHashCodeNotEqual()
    {
        Expression a = new() { Nonterminal.Get("a"), Terminal.Get("b"), Nonterminal.Get("c") };
        Expression b = new() { Terminal.Get("q"), Nonterminal.Get("f"), EmptyString.Instance };
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
