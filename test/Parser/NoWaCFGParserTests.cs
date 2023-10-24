using NoWa.Common;

namespace NoWa.Parser.Tests;

/// <summary>
/// Contains unit tests for the <see cref="NoWaCFGParser"/> class.
/// </summary>
[TestClass]
public class NoWaCFGParserTests
{
    /// <summary>
    /// Tests the parsing of valid grammars.
    /// Valid grammars are compared to their <see cref="CFG.ToString"/> method, which should be identical to the contents of the file itself.
    /// </summary>
    /// <param name="file">The file to parse.</param>
    [DataTestMethod]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidCFG1")]
    [DataRow("ValidCFG1")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidCFG2")]
    [DataRow("ValidCFG2")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidCFG3")]
    [DataRow("ValidCFG3")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidCFG4")]
    [DataRow("ValidCFG4")]
    public void TestParseValid(string file)
    {
        CFG grammar = NoWaCFGParser.Parse(file);
        Assert.AreEqual(File.ReadAllText(file).TrimEnd(), grammar.ToString());
    }
}
