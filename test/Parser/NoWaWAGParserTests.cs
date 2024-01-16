using NoWa.Common;

namespace NoWa.Parser.Tests;

/// <summary>
/// Contains unit tests for the <see cref="NoWaWAGParser"/>
/// </summary>
[TestClass]
public class NoWaWAGParserTests
{
    /// <summary>
    /// Tests the parsing of valid grammars that are identical to their <see cref="WAG.ToString"/> representation.
    /// Valid grammars are compared to their <see cref="WAG.ToString"/> method, which should be identical to the contents of the file itself.
    /// </summary>
    /// <param name="file">The file to parse.</param>
    [DataTestMethod]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\ValidWAG1")]
    [DataRow("ValidWAG1")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\ValidWAG2")]
    [DataRow("ValidWAG2")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\ValidWAG3")]
    [DataRow("ValidWAG3")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\ValidWAG4")]
    [DataRow("ValidWAG4")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\ValidWAG5")]
    [DataRow("ValidWAG5")]
    public void TestParseValid(string file)
    {
        WAG grammar = NoWaWAGParser.Parse(file);
        Assert.AreEqual(File.ReadAllText(file).TrimEnd(), grammar.ToString());
    }

    /// <summary>
    /// Tests whether different decimal separators are properly parsed.
    /// </summary>
    [TestMethod]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\DecimalSeparatorsWAG")]
    public void TestDecimalSeparators()
    {
        WAG grammar = NoWaWAGParser.Parse("DecimalSeparatorsWAG");
        Assert.AreEqual(
            $"RULE -2.6-> 'yes' ;{Environment.NewLine}" +
            $"RULE -4.5-> 'yes' ;", grammar.ToString());
    }

    /// <summary>
    /// Tests whether default weights are properly applied.
    /// </summary>
    [TestMethod]
    [DeploymentItem($@"TestFiles\{nameof(NoWaWAGParserTests)}\DefaultWeightsWAG")]
    public void TestDefaultWeights()
    {
        WAG grammar = NoWaWAGParser.Parse("DefaultWeightsWAG");
        Assert.AreEqual(
            $"RULE -1-> 'yes' ;{Environment.NewLine}" +
            $"RULE -1-> 'yes' ;", grammar.ToString());
    }
}
