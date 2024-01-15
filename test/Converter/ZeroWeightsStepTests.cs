using NoWa.Common;

namespace NoWa.Converter.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="ZeroWeightsStep"/>.
    /// </summary>
    [TestClass]
    public class ZeroWeightsStepTests
    {
        /// <summary>
        /// Test the removal of zero-weight productions.
        /// </summary>
        [TestMethod]
        public void TestRemoveZeroWeights()
        {
            WAG wag = new();

            wag.AddProduction(new Production(Nonterminal.Get("S"), 1, Nonterminal.Get("A"), Terminal.Get("b")));
            wag.AddProduction(new Production(Nonterminal.Get("S"), 0, Terminal.Get("x")));

            wag.AddProduction(new Production(Nonterminal.Get("A"), 0.5, Terminal.Get("a")));
            wag.AddProduction(new Production(Nonterminal.Get("A"), 0.5, Terminal.Get("a"), Terminal.Get("a")));
            wag.AddProduction(new Production(Nonterminal.Get("A"), 0, EmptyString.Instance));

            ZeroWeightsStep step = new(new TestLogger());
            step.Convert(wag);

            Assert.AreEqual(
                $"S -1-> A 'b' ;{Environment.NewLine}" +
                $"A -0.5-> 'a' ;{Environment.NewLine}" +
                $"A -0.5-> 'a' 'a' ;", wag.ToString());
        }

        /// <summary>
        /// Test the removal of zero-weight productions, where weights that are attribute references must be kept.
        /// </summary>
        [TestMethod]
        public void TestKeepAttributeWeights()
        {
            WAG wag = new();

            wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'a');

            wag.AddProduction(new Production(Nonterminal.Get("S"), 1, Nonterminal.Get("A"), Terminal.Get("b")));
            wag.AddProduction(new Production(Nonterminal.Get("S"), 0, Terminal.Get("x")));

            wag.AddProduction(new Production(Nonterminal.Get("A"), 0.5, Terminal.Get("a")));
            wag.AddProduction(new Production(Nonterminal.Get("A"), "$a", Terminal.Get("a"), Terminal.Get("a")));
            wag.AddProduction(new Production(Nonterminal.Get("A"), 0, EmptyString.Instance));

            ZeroWeightsStep step = new(new TestLogger());
            step.Convert(wag);

            Assert.AreEqual(
                $"S -1-> A{{a}} 'b' ;{Environment.NewLine}" +
                $"A{{;a;}} -0.5-> 'a' ;{Environment.NewLine}" +
                $"A{{;a;}} -$a-> 'a' 'a' ;", wag.ToString());
        }

        /// <summary>
        /// Tests the step on a grammar without weights.
        /// </summary>
        [TestMethod]
        public void TestRemoveNoWeights()
        {
            WAG wag = new();

            wag.AddSynthesizedAttribute(Nonterminal.Get("A"), 'a');

            wag.AddProduction(new Production(Nonterminal.Get("S"), 1, Nonterminal.Get("A"), Terminal.Get("b")));
            wag.AddProduction(new Production(Nonterminal.Get("S"), 0.5, Terminal.Get("x")));

            wag.AddProduction(new Production(Nonterminal.Get("A"), 0.5, Terminal.Get("a")));
            wag.AddProduction(new Production(Nonterminal.Get("A"), "$a", Terminal.Get("a"), Terminal.Get("a")));
            wag.AddProduction(new Production(Nonterminal.Get("A"), 0.5, EmptyString.Instance));

            ZeroWeightsStep step = new(new TestLogger());
            step.Convert(wag);

            Assert.AreEqual(
                $"S -1-> A{{a}} 'b' ;{Environment.NewLine}" +
                $"S -0.5-> 'x' ;{Environment.NewLine}" +
                $"A{{;a;}} -0.5-> 'a' ;{Environment.NewLine}" +
                $"A{{;a;}} -$a-> 'a' 'a' ;{Environment.NewLine}" +
                $"A{{;a;}} -0.5-> '' ;", wag.ToString());
        }
    }
}