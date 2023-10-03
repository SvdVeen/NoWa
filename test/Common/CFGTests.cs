namespace NoWa.Common.Tests;

/// <summary>
/// Contains unit tests for the <see cref="CFG"/> class.
/// </summary>
[TestClass]
public class CFGTests
{
    /// <summary>
    /// Tests an empty grammar.
    /// </summary>
    [TestMethod]
    public void TestEmptyGrammar()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.Productions.Count, "Production count does not match.");
        Assert.AreEqual(0, grammar.Nonterminals.Count, "Nonterminal count does not match.");
        Assert.AreEqual(0, grammar.Terminals.Count, "Terminal count does not match.");
        Assert.AreEqual("Empty grammar", grammar.ToString(), "ToString does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.Clear"/> method.
    /// </summary>
    [TestMethod]
    public void TestClear()
    {
        CFG grammar = new();
        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        grammar.Clear();

        Assert.AreEqual(0, grammar.Productions.Count, "Production count does not match.");
        Assert.AreEqual(0, grammar.Nonterminals.Count, "Nonterminal count does not match.");
        Assert.AreEqual(0, grammar.Terminals.Count, "Terminal count does not match.");
        Assert.AreEqual("Empty grammar", grammar.ToString(), "ToString does not match.");
    }

    /// <summary>
    /// Tests the <see cref="CFG.ToString"/> method for a non-empty grammar.
    /// </summary>
    [TestMethod]
    public void TestToStringNotEmpty()
    {
        CFG grammar = new();

        grammar.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("B"), Terminal.Get("c")));
        grammar.AddProduction(new(Nonterminal.Get("S"), EmptyString.Instance));

        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));

        Assert.AreEqual(
            $"S = B 'c' | '' ;{Environment.NewLine}" +
            $"B = 'b' ;", grammar.ToString());
    }

    #region Terminals

    /// <summary>
    /// Tests the <see cref="CFG.Terminals"/> property after adding a terminal.
    /// </summary>
    [TestMethod]
    public void TestTerminals()
    {
        CFG grammar = new();
        Terminal a = Terminal.Get("a");
        _ = grammar.AddTerminal(a);
        Assert.AreSame(a, grammar.Terminals[0]);
    }

    /// <summary>
    /// Tests the <see cref="CFG.Terminals"/> property's count after adding and removing various terminals.
    /// </summary>
    [TestMethod]
    public void TestTerminalsCount()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.Terminals.Count);

        _ = grammar.AddTerminal(Terminal.Get("a"));
        Assert.AreEqual(1, grammar.Terminals.Count);

        _ = grammar.AddTerminal(Terminal.Get("b"));
        Assert.AreEqual(2, grammar.Terminals.Count);

        grammar.RemoveTerminalAt(1);
        Assert.AreEqual(1, grammar.Terminals.Count);

        grammar.RemoveTerminalAt(0);
        Assert.AreEqual(0, grammar.Terminals.Count);
    }

    /// <summary>
    /// Tests whether <see cref="CFG.Nonterminals"/> throws an exception for an index outside the acceptable range.
    /// </summary>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestTerminalsIndexOutOfRange(int index)
    {
        CFG grammar = new();
        _ = grammar.AddTerminal(Terminal.Get("a"));
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.Terminals[index]);
    }

    /// <summary>
    /// Tests adding a terminal with the <see cref="CFG.AddTerminal(Terminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddTerminal()
    {
        CFG grammar = new();
        Terminal a = Terminal.Get("a");
        Assert.IsTrue(grammar.AddTerminal(a));
    }

    /// <summary>
    /// Tests adding a terminal twice with the <see cref="CFG.AddTerminal(Terminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddTerminalTwice()
    {
        CFG grammar = new();
        Terminal a = Terminal.Get("a");
        _ = grammar.AddTerminal(a);
        Assert.IsFalse(grammar.AddTerminal(a));
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsTerminal(Terminal)"/> method for a terminal that exists in the grammar.
    /// </summary>
    [TestMethod]
    public void TestContainsTerminal()
    {
        CFG grammar = new();
        Terminal a = Terminal.Get("a");
        _ = grammar.AddTerminal(a);
        Assert.IsTrue(grammar.ContainsTerminal(a));
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsTerminal(Terminal)"/> method for a terminal that does not exist in the grammar.
    /// </summary>
    [TestMethod]
    public void TestDoesNotContainTerminal()
    {
        CFG grammar = new();
        _ = grammar.AddTerminal(Terminal.Get("b"));
        Assert.IsFalse(grammar.ContainsTerminal(Terminal.Get("a")));
    }

    #endregion Terminals

    #region Nonterminals

    /// <summary>
    /// Tests the <see cref="CFG.Nonterminals"/> property after adding a nonterminal.
    /// </summary>
    [TestMethod]
    public void TestNonterminals()
    {
        CFG grammar = new();
        Nonterminal a = Nonterminal.Get("a");
        _ = grammar.AddNonterminal(a);
        Assert.AreSame(a, grammar.Nonterminals[0]);
    }

    /// <summary>
    /// Tests the <see cref="CFG.Nonterminals"/>.Count property after adding and removing various nonterminals.
    /// </summary>
    [TestMethod]
    public void TestNonterminalsCount()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.Nonterminals.Count);

        _ = grammar.AddNonterminal(Nonterminal.Get("a"));
        Assert.AreEqual(1, grammar.Nonterminals.Count);

        _ = grammar.AddNonterminal(Nonterminal.Get("b"));
        Assert.AreEqual(2, grammar.Nonterminals.Count);

        grammar.RemoveNonterminalAt(1);
        Assert.AreEqual(1, grammar.Nonterminals.Count);

        grammar.RemoveNonterminalAt(0);
        Assert.AreEqual(0, grammar.Nonterminals.Count);
    }

    /// <summary>
    /// Tests whether <see cref="CFG.Nonterminals"/> throws an exception for an index outside the acceptable range.
    /// </summary>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestNonterminalsIndexOutOfRange(int index)
    {
        CFG grammar = new();
        _ = grammar.AddNonterminal(Nonterminal.Get("a"));
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.Nonterminals[index]);
    }

    /// <summary>
    /// Tests adding a nonterminal with the <see cref="CFG.AddNonterminal(Nonterminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddNonterminal()
    {
        CFG grammar = new();
        Nonterminal A = Nonterminal.Get("A");
        Assert.IsTrue(grammar.AddNonterminal(A));
    }

    /// <summary>
    /// Tests adding a nonterminal twice with the <see cref="CFG.AddNonterminal(Nonterminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddNonterminalTwice()
    {
        CFG grammar = new();
        Nonterminal A = Nonterminal.Get("A");
        _ = grammar.AddNonterminal(A);
        Assert.IsFalse(grammar.AddNonterminal(A));
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsNonterminal(Nonterminal)"/> method for a nonterminal that exists in the grammar.
    /// </summary>
    [TestMethod]
    public void TestContainsNonterminal()
    {
        CFG grammar = new();
        Nonterminal A = Nonterminal.Get("A");
        _ = grammar.AddNonterminal(A);
        Assert.IsTrue(grammar.ContainsNonterminal(A));
    }

    /// <summary>
    /// Tests the <see cref="CFG.ContainsNonterminal(Nonterminal)"/> method for a nonterminal that does not exist in the grammar.
    /// </summary>
    [TestMethod]
    public void TestDoesNotContainNonterminal()
    {
        CFG grammar = new();
        _ = grammar.AddNonterminal(Nonterminal.Get("B"));
        Assert.IsFalse(grammar.ContainsNonterminal(Nonterminal.Get("A")));
    }

    #endregion Nonterminals

    #region Productions

    /// <summary>
    /// Tests the <see cref="CFG.AddProduction(Production)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddProduction()
    {
        CFG grammar = new();

        Production production = new(Nonterminal.Get("S"), Terminal.Get("t"));
        grammar.AddProduction(production);
        Assert.AreSame(production, grammar.Productions[0]);
    }

    /// <summary>
    /// Tests whether the <see cref="CFG.AddProduction(Production)"/> method properly adds symbols to the grammar.
    /// </summary>
    [TestMethod]
    public void TestAddProductionSymbols()
    {
        CFG grammar = new();
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("s"), Nonterminal.Get("A")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("b")));
        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));

        Assert.AreEqual(Nonterminal.Get("S"), grammar.Nonterminals[0]);
        Assert.AreEqual(Nonterminal.Get("A"), grammar.Nonterminals[1]);
        Assert.AreEqual(Terminal.Get("s"), grammar.Terminals[0]);
        Assert.AreEqual(Terminal.Get("b"), grammar.Terminals[1]);
        Assert.AreEqual(Terminal.Get("a"), grammar.Terminals[2]);
        Assert.AreEqual(2, grammar.Nonterminals.Count);
        Assert.AreEqual(3, grammar.Terminals.Count);
    }

    /// <summary>
    /// Tests the <see cref="CFG.Productions"/> property's count after adding and removing various productions.
    /// </summary>
    [TestMethod]
    public void TestProductionsCount()
    {
        CFG grammar = new();
        Assert.AreEqual(0, grammar.Productions.Count);

        grammar.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")));
        Assert.AreEqual(1, grammar.Productions.Count);

        grammar.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));
        Assert.AreEqual(2, grammar.Productions.Count);

        grammar.RemoveProduction(grammar.Productions[0]);
        Assert.AreEqual(1, grammar.Productions.Count);

        grammar.RemoveProductionAt(0);
        Assert.AreEqual(0, grammar.Productions.Count);
    }

    /// <summary>
    /// Tests whether <see cref="CFG.Productions"/> throws an exception for an index outside the acceptable range.
    /// </summary>
    [TestMethod]
    [DataTestMethod]
    [DataRow(int.MinValue)]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void TestProductionsIndexOutOfRange(int index)
    {
        CFG grammar = new();
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("s")));
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => grammar.Productions[index]);
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetProductionsByHead(Nonterminal)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetProductionsByHead()
    {
        CFG grammar = new();

        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("b"), Terminal.Get("c")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("d")));

        var productionsByHead = grammar.GetProductionsByHead(Nonterminal.Get("S"));

        // Simultaneously overdoing it and not hitting everything.
        Assert.AreEqual(3, productionsByHead.Count);
        Assert.AreEqual(Nonterminal.Get("S"), productionsByHead[0].Head);
        Assert.AreEqual(Terminal.Get("a"), productionsByHead[0].Body[0]);
        Assert.AreEqual(Nonterminal.Get("S"), productionsByHead[1].Head);
        Assert.AreEqual(Terminal.Get("b"), productionsByHead[1].Body[0]);
        Assert.AreEqual(Terminal.Get("c"), productionsByHead[1].Body[1]);
        Assert.AreEqual(Nonterminal.Get("S"), productionsByHead[2].Head);
        Assert.AreEqual(Terminal.Get("d"), productionsByHead[2].Body[0]);
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetProductionsByHead(Nonterminal)"/> method for a nonterminal that has no productions.
    /// </summary>
    [TestMethod]
    public void TestGetProductionsByHeadNone()
    {
        CFG grammar = new();

        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("a")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("b"), Terminal.Get("c")));
        grammar.AddProduction(new(Nonterminal.Get("S"), Terminal.Get("d")));

        var productionsByHead = grammar.GetProductionsByHead(Nonterminal.Get("A"));

        Assert.AreEqual(0, productionsByHead.Count);
    }

    /// <summary>
    /// Tests the <see cref="CFG.GetProductionsByHead(Nonterminal)"/> method for a nonterminal that has no productions.
    /// </summary>
    [TestMethod]
    public void TestGetProductionsByHeadAfterRemoval()
    {
        CFG grammar = new();

        Production prod1 = new(Nonterminal.Get("S"), Terminal.Get("a"));
        grammar.AddProduction(prod1);

        Production prod2 = new(Nonterminal.Get("S"), Terminal.Get("b"), Terminal.Get("c"));
        grammar.AddProduction(prod2);

        Production prod3 = new(Nonterminal.Get("S"), Terminal.Get("d"));
        grammar.AddProduction(prod3);

        // I would have split this into two separate test cases if I was less lazy.
        grammar.RemoveProduction(prod1);
        grammar.RemoveProductionAt(0);

        var productionsByHead = grammar.GetProductionsByHead(Nonterminal.Get("S"));

        Assert.AreEqual(1, productionsByHead.Count);
        Assert.AreEqual(prod3, productionsByHead[0]);
    }

    /// <summary>
    /// Tests the <see cref="CFG.RemoveProduction(Production)"/> method.
    /// </summary>
    [TestMethod]
    public void TestRemoveProduction()
    {
        CFG grammar = new();
        Production production = new(Nonterminal.Get("S"), Terminal.Get("a"));
        grammar.AddProduction(production);

        Assert.IsTrue(grammar.RemoveProduction(production));
    }

    /// <summary>
    /// Tests the <see cref="CFG.RemoveProduction(Production)"/> method for removing a production from an empty grammar.
    /// </summary>
    [TestMethod]
    public void TestRemoveProductionFromEmptyGrammar()
    {
        CFG grammar = new();

        Assert.IsFalse(grammar.RemoveProduction(new(Nonterminal.Get("S"), Terminal.Get("a"))));
    }

    /// <summary>
    /// Tests the <see cref="CFG.RemoveProduction(Production)"/> method.
    /// </summary>
    [TestMethod]
    public void TestRemoveProductionNotInGrammar()
    {
        CFG grammar = new();
        Production production = new(Nonterminal.Get("S"), Terminal.Get("a"));
        grammar.AddProduction(production);

        Assert.IsFalse(grammar.RemoveProduction(new(Nonterminal.Get("A"), Terminal.Get("a"))));
    }
    #endregion Productions
}
