using NuGet.Frameworks;

namespace NoWa.Common.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="WAG"/> class.
    /// </summary>
    [TestClass]
    public class WAGTests
    {
        #region Weights
        /// <summary>
        /// Tests the <see cref="WAG.AddProduction(Production)"/> method.
        /// </summary>
        [TestMethod]
        public void AddProductionTest()
        {
            WAG wag = new();
            Production A = new(Nonterminal.Get("A"), Terminal.Get("a"));
            wag.AddProduction(A);

            Assert.AreSame(A, wag.Productions[0]);
        }

        /// <summary>
        /// Tests the <see cref="WAG.AddProduction(Production, double)"/> method.
        /// </summary>
        [TestMethod]
        public void AddProductionWithWeightTest()
        {
            WAG wag = new();
            Production A = new(Nonterminal.Get("A"), 5.2, Terminal.Get("a"));
            wag.AddProduction(A);

            Assert.AreSame(A, wag.Productions[0]);
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveProduction(Production)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveProductionTest()
        {
            WAG wag = new();
            Production A = new(Nonterminal.Get("A"), 5.2, Terminal.Get("a"));

            wag.RemoveProduction(A);

            Assert.AreEqual(0, wag.Productions.Count);
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveProductionAt(int)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveProductionAtTest()
        {
            WAG wag = new();
            wag.AddProduction(new(Nonterminal.Get("A"), 5.2, Terminal.Get("a")));
            Production B = new(Nonterminal.Get("B"), Terminal.Get("b"));
            wag.AddProduction(B);

            wag.RemoveProductionAt(1);

            Assert.AreEqual(1, wag.Productions.Count);
            Assert.AreNotSame(B, wag.Productions[0]);
        }
        #endregion Weights

        #region Attributes
        /// <summary>
        /// Tests the <see cref="WAG.AddNonterminal(Nonterminal)"/> method.
        /// </summary>
        [TestMethod]
        public void AddNonterminalTest()
        {
            WAG wag = new();

            Assert.IsTrue(wag.AddNonterminal(Nonterminal.Get("A")));
            Assert.AreEqual(1, wag.Nonterminals.Count);
            Assert.AreSame(Nonterminal.Get("A"), wag.Nonterminals[0]);

            Assert.IsFalse(wag.AddNonterminal(Nonterminal.Get("A")));
        }
        #endregion Attributes

        /// <summary>
        /// Tests the <see cref="WAG.Clear()"/> method.
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            WAG wag = new();
            wag.AddProduction(new(Nonterminal.Get("A"), 5.2, Terminal.Get("a")));
            wag.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));

            wag.Clear();

            Assert.AreEqual("Empty WAG", wag.ToString());
            Assert.AreEqual(0, wag.Productions.Count);
            Assert.AreEqual(0, wag.Nonterminals.Count);
            Assert.AreEqual(0, wag.Terminals.Count);
        }

        /// <summary>
        /// Tests the <see cref="WAG.ToString"/> method.
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            WAG wag = new();
            wag.AddProduction(new(Nonterminal.Get("S"), 10, Nonterminal.Get("A"), Nonterminal.Get("C")));
            wag.AddProduction(new(Nonterminal.Get("A"), 4, Terminal.Get("a")));
            wag.AddProduction(new(Nonterminal.Get("A"), 6, Terminal.Get("b")));
            wag.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

            Nonterminal.Get("A").InheritedAttributes.Add('p');
            Nonterminal.Get("A").SynthesizedAttributes.Add('q');

            Assert.AreEqual(
                $"S -10-> A{{p,q}} C ;{Environment.NewLine}" +
                $"A{{p;q}} -4-> 'a' ;{Environment.NewLine}" +
                $"A{{p;q}} -6-> 'b' ;{Environment.NewLine}" +
                $"C -1-> 'c' ;", wag.ToString());
        }
    }
}