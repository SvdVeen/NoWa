﻿using NuGet.Frameworks;

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
        #region Inherited
        /// <summary>
        /// Tests adding an inherited attribute.
        /// Checks if it is part of the set of all inherited attributes,
        /// and part of the set of attributes for the nonterminal it was added to.
        /// </summary>
        [TestMethod]
        public void TestAddInheritedAttribute()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            Assert.IsTrue(wag.AddInheritedAttribute(Nonterminal.Get("S"), 'a'));
            Assert.IsTrue(wag.GetInheritedAttributes().Contains('a'));
            Assert.IsTrue(wag.GetInheritedAttributes(Nonterminal.Get("S")).Contains('a'));
        }

        /// <summary>
        /// Tests that an inherited attribute cannot also be added as a synthesized attribute.
        /// </summary>
        [TestMethod]
        public void TestAddInheritedAsSynthesized()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            wag.AddInheritedAttribute(Nonterminal.Get("S"), 'a');
            Assert.IsFalse(wag.AddSynthesizedAttribute(Nonterminal.Get("S"), 'a'));
        }

        /// <summary>
        /// Tests that an inherited attribute cannot also be added as a static attribute.
        /// </summary>
        [TestMethod]
        public void TestAddInheritedAsStatic()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            wag.AddInheritedAttribute(Nonterminal.Get("S"), 'a');
            Assert.IsFalse(wag.AddStaticAttribute(Nonterminal.Get("S"), 'a'));
        }
        #endregion Inherited

        #region Synthesized
        /// <summary>
        /// Tests adding a synthesized attribute.
        /// Checks if it is part of the set of all synthesized attributes,
        /// and part of the set of attributes for the nonterminal it was added to.
        /// </summary>
        [TestMethod]
        public void TestAddSynthesizedAttribute()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            Assert.IsTrue(wag.AddSynthesizedAttribute(Nonterminal.Get("S"), 'a'));
            Assert.IsTrue(wag.GetSynthesizedAttributes().Contains('a'));
            Assert.IsTrue(wag.GetSynthesizedAttributes(Nonterminal.Get("S")).Contains('a'));
        }

        /// <summary>
        /// Tests that a synthesized attribute cannot also be added as an inherited attribute.
        /// </summary>
        [TestMethod]
        public void TestAddSynthesizedAsInherited()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            wag.AddSynthesizedAttribute(Nonterminal.Get("S"), 'a');
            Assert.IsFalse(wag.AddInheritedAttribute(Nonterminal.Get("S"), 'a'));
        }

        /// <summary>
        /// Tests that a synthesized attribute cannot also be added as a static attribute.
        /// </summary>
        [TestMethod]
        public void TestAddSynthesizedAsStatic()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            wag.AddSynthesizedAttribute(Nonterminal.Get("S"), 'a');
            Assert.IsFalse(wag.AddStaticAttribute(Nonterminal.Get("S"), 'a'));
        }
        #endregion Synthesized

        #region Static
        /// <summary>
        /// Tests adding a static attribute.
        /// Checks if it is part of the set of all static attributes,
        /// and part of the set of attributes for the nonterminal it was added to.
        /// </summary>
        [TestMethod]
        public void TestAddStaticAttribute()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            Assert.IsTrue(wag.AddStaticAttribute(Nonterminal.Get("S"), 'a'));
            Assert.IsTrue(wag.GetStaticAttributes().Contains('a'));
            Assert.IsTrue(wag.GetStaticAttributes(Nonterminal.Get("S")).Contains('a'));
        }

        /// <summary>
        /// Tests that a static attribute cannot also be added as an inherited attribute.
        /// </summary>
        [TestMethod]
        public void TestAddStaticAsInherited()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            wag.AddStaticAttribute(Nonterminal.Get("S"), 'a');
            Assert.IsFalse(wag.AddInheritedAttribute(Nonterminal.Get("S"), 'a'));
        }

        /// <summary>
        /// Tests that a static attribute cannot also be added as a synthesized attribute.
        /// </summary>
        [TestMethod]
        public void TestAddStaticAsSynthesized()
        {
            WAG wag = new();

            wag.AddNonterminal(Nonterminal.Get("S"));

            wag.AddStaticAttribute(Nonterminal.Get("S"), 'a');
            Assert.IsFalse(wag.AddSynthesizedAttribute(Nonterminal.Get("S"), 'a'));
        }
        #endregion Static
        #endregion Attributes

        /// <summary>
        /// Tests whether adding the first production properly sets the start symbol.
        /// </summary>
        [TestMethod]
        public void TestAddProductionStartSymbol()
        {
            WAG grammar = new();

            grammar.AddProduction(new Production(Nonterminal.Get("S"), EmptyString.Instance));

            Assert.AreSame(Nonterminal.Get("S"), grammar.StartSymbol);
        }

        /// <summary>
        /// Tests whether adding a second production keeps the start symbol the same.
        /// </summary>
        [TestMethod]
        public void TestAddSecondProductionStartSymbol()
        {
            WAG grammar = new();

            grammar.AddProduction(new Production(Nonterminal.Get("S"), EmptyString.Instance));
            grammar.AddProduction(new Production(Nonterminal.Get("A"), EmptyString.Instance));

            Assert.AreSame(Nonterminal.Get("S"), grammar.StartSymbol);
        }

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

            Assert.IsNull(wag.StartSymbol);
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

            wag.AddInheritedAttribute(Nonterminal.Get("A"), 'p');
            wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'q');
            wag.AddStaticAttribute(Nonterminal.Get("A"), 'r');

            Assert.AreEqual(
                $"S -10-> A{{p,q,r}} C ;{Environment.NewLine}" +
                $"A{{p;q;r}} -4-> 'a' ;{Environment.NewLine}" +
                $"A{{p;q;r}} -6-> 'b' ;{Environment.NewLine}" +
                $"C -1-> 'c' ;", wag.ToString());
        }
    }
}