using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NoWa.Common;
using GLexer = NoWa.Parser.Generated.NoWaLexer;
using GParser = NoWa.Parser.Generated.NoWaParser;

namespace NoWa.Parser;

/// <summary>
/// Parses a grammar from a file and converts it to a <see cref="Grammar"/>.
/// </summary>
public class NoWaParser
{
    /// <summary>
    /// Parse a grammar from a file and convert it to a <see cref="Grammar"/>.
    /// </summary>
    /// <param name="path">The path to the file to parse.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">The file path was not valid.</exception>
    public Grammar Parse(string path)
    {
        if (!Path.Exists(path))
            throw new FileNotFoundException($"Could not find the file: {path}");

        var inputStream = CharStreams.fromPath(path);
        var lexer = new GLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new GParser(tokens) { BuildParseTree = true };

        var listener = new NoWaListener();
        var walker = new ParseTreeWalker();
        walker.Walk(listener, parser.grammar_());

        return listener.Grammar;
    }
}
