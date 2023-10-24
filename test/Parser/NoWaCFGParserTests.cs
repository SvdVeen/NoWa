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
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidGrammar1")]
    [DataRow("ValidGrammar1")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidGrammar2")]
    [DataRow("ValidGrammar2")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidGrammar3")]
    [DataRow("ValidGrammar3")]
    [DeploymentItem($@"TestFiles\{nameof(NoWaCFGParserTests)}\ValidGrammar4")]
    [DataRow("ValidGrammar4")]
    public void ParseValid(string file)
    {
        CFG grammar = NoWaCFGParser.Parse(file);
        Assert.AreEqual(File.ReadAllText(file).TrimEnd(), grammar.ToString());
    }
}
