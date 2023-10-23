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
            Assert.AreEqual(1, wag.Weights[0]);
        }

        /// <summary>
        /// Tests the <see cref="WAG.AddProduction(Production, double)"/> method.
        /// </summary>
        [TestMethod]
        public void AddProductionWithWeightTest()
        {
            WAG wag = new();
            Production A = new(Nonterminal.Get("A"), Terminal.Get("a"));
            wag.AddProduction(A, 5.2);

            Assert.AreSame(A, wag.Productions[0]);
            Assert.AreEqual(5.2, wag.Weights[0]);
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveProduction(Production)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveProductionTest()
        {
            WAG wag = new();
            Production A = new(Nonterminal.Get("A"), Terminal.Get("a"));
            wag.AddProduction(A, 5.2);

            wag.RemoveProduction(A);

            Assert.AreEqual(0, wag.Productions.Count);
            Assert.AreEqual(0, wag.Weights.Count);
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveProductionAt(int)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveProductionAtTest()
        {
            WAG wag = new();
            wag.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")), 5.2);
            Production B = new(Nonterminal.Get("B"), Terminal.Get("b"));
            wag.AddProduction(B);

            wag.RemoveProductionAt(1);

            Assert.AreEqual(1, wag.Productions.Count);
            Assert.AreNotSame(B, wag.Productions[0]);
            Assert.AreEqual(1, wag.Weights.Count);
            Assert.AreEqual(1, wag.Weights[0]);
        }

        [TestMethod]
        public void SetWeightTest()
        {
            WAG wag = new();
            Production A = new(Nonterminal.Get("A"), Terminal.Get("a"));
            wag.AddProduction(A, 5.2);

            wag.SetWeight(A, 0.53);

            Assert.AreEqual(0.53, wag.Weights[0]);
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

            Assert.AreEqual(0, wag.GetInheritedAttributes(Nonterminal.Get("A")).Count);
            Assert.AreEqual(0, wag.GetSynthesizedAttributes(Nonterminal.Get("A")).Count);
            Assert.AreEqual(0, wag.GetStaticAttributes(Nonterminal.Get("A")).Count);

            Assert.IsFalse(wag.AddNonterminal(Nonterminal.Get("A")));
        }

        /// <summary>
        /// Tests the <see cref="WAG.AddInheritedAttribute(Nonterminal, char)"/> and <see cref="WAG.GetInheritedAttributes(Nonterminal)"/> methods.
        /// </summary>
        [TestMethod]
        public void InheritedAttributeTest()
        {
            WAG wag = new();
            wag.AddNonterminal(Nonterminal.Get("A"));

            Assert.IsTrue(wag.AddInheritedAttribute(Nonterminal.Get("A"), 'a'));

            var attrs = wag.GetInheritedAttributes(Nonterminal.Get("A"));
            Assert.AreEqual(1, attrs.Count);
            Assert.IsTrue(attrs.Contains('a'));
        }

        /// <summary>
        /// Tests the <see cref="WAG.AddSynthesizedAttribute(Nonterminal, char)"/> and <see cref="WAG.GetSynthesizedAttributes(Nonterminal)"/> methods.
        /// </summary>
        [TestMethod]
        public void SynthesizedAttributeTest()
        {
            WAG wag = new();
            wag.AddNonterminal(Nonterminal.Get("A"));

            Assert.IsTrue(wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'a'));

            var attrs = wag.GetSynthesizedAttributes(Nonterminal.Get("A"));
            Assert.AreEqual(1, attrs.Count);
            Assert.IsTrue(attrs.Contains('a'));
        }


        /// <summary>
        /// Tests the <see cref="WAG.GetStaticAttributes(Nonterminal)"/> method.
        /// </summary>
        [TestMethod]
        public void GetStaticAttributesTest()
        {
            WAG wag = new();
            wag.AddNonterminal(Nonterminal.Get("A"));

            wag.AddInheritedAttribute(Nonterminal.Get("A"), 'a');
            wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'b');

            var attrs = wag.GetStaticAttributes(Nonterminal.Get("A"));
            Assert.AreEqual(2, attrs.Count);
            Assert.IsTrue(attrs.Contains('a'));
            Assert.IsTrue(attrs.Contains('b'));
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveInheritedAttribute(Nonterminal, char)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveInheritedAttributeTest()
        {
            WAG wag = new();
            wag.AddNonterminal(Nonterminal.Get("A"));

            wag.AddInheritedAttribute(Nonterminal.Get("A"), 'a');

            Assert.IsTrue(wag.RemoveInheritedAttribute(Nonterminal.Get("A"), 'a'));
            Assert.IsFalse(wag.RemoveInheritedAttribute(Nonterminal.Get("A"), 'a'));
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveSynthesizedAttribute(Nonterminal, char)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveSynthesizedAttributeTest()
        {
            WAG wag = new();
            wag.AddNonterminal(Nonterminal.Get("A"));

            wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'a');

            Assert.IsTrue(wag.RemoveSynthesizedAttribute(Nonterminal.Get("A"), 'a'));
            Assert.IsFalse(wag.RemoveSynthesizedAttribute(Nonterminal.Get("A"), 'a'));
        }

        /// <summary>
        /// Tests the <see cref="WAG.RemoveNonterminalAt(int)"/> method.
        /// </summary>
        [TestMethod]
        public void RemoveNonterminalAtTest()
        {
            WAG wag = new();
            wag.AddNonterminal(Nonterminal.Get("A"));
            wag.AddNonterminal(Nonterminal.Get("B"));
            wag.AddSynthesizedAttribute(Nonterminal.Get("B"), 'b');

            wag.RemoveNonterminalAt(1);

            Assert.AreEqual(1, wag.Nonterminals.Count);
            Assert.AreNotSame(Nonterminal.Get("B"), wag.Nonterminals[0]);
            Assert.AreEqual(0, wag.GetInheritedAttributes(Nonterminal.Get("B")).Count);
            Assert.AreEqual(0, wag.GetSynthesizedAttributes(Nonterminal.Get("B")).Count);
            Assert.AreEqual(0, wag.GetStaticAttributes(Nonterminal.Get("B")).Count);
        }
        #endregion Attributes

        /// <summary>
        /// Tests the <see cref="WAG.Clear()"/> method.
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            WAG wag = new();
            wag.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")), 5.2);
            wag.AddProduction(new(Nonterminal.Get("B"), Terminal.Get("b")));
            wag.AddInheritedAttribute(Nonterminal.Get("A"), 'd');
            wag.AddSynthesizedAttribute(Nonterminal.Get("B"), 'm');

            wag.Clear();

            Assert.AreEqual("Empty WAG", wag.ToString());
            Assert.AreEqual(0, wag.Productions.Count);
            Assert.AreEqual(0, wag.Nonterminals.Count);
            Assert.AreEqual(0, wag.Terminals.Count);
            Assert.AreEqual(0, wag.Weights.Count);
            Assert.AreEqual(0, wag.GetInheritedAttributes(Nonterminal.Get("A")).Count);
            Assert.AreEqual(0, wag.GetSynthesizedAttributes(Nonterminal.Get("B")).Count);
        }

        [TestMethod]
        public void ToStringTest()
        {
            WAG wag = new();
            wag.AddProduction(new(Nonterminal.Get("S"), Nonterminal.Get("A"), Nonterminal.Get("C")), 10);
            wag.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("a")), 4);
            wag.AddProduction(new(Nonterminal.Get("A"), Terminal.Get("b")), 6);
            wag.AddProduction(new(Nonterminal.Get("C"), Terminal.Get("c")));

            wag.AddInheritedAttribute(Nonterminal.Get("A"), 'p');
            wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'q');

            Assert.AreEqual(
                $"S -10-> A{{p,q}} C ;{Environment.NewLine}" +
                $"A{{p;q}} -4-> 'a' ;{Environment.NewLine}" +
                $"A{{p;q}} -6-> 'b' ;{Environment.NewLine}" +
                $"C -1-> 'c' ;", wag.ToString());
        }
    }
}