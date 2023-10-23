using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NoWa.Common;
using WAGLexer = NoWa.Parser.Generated.NoWaWAGLexer;
using WAGParser = NoWa.Parser.Generated.NoWaWAGParser;

namespace NoWa.Parser;

/// <summary>
/// Parses a grammar from a file and converts it to a <see cref="WAG"/>.
/// </summary>
public static class NoWaWAGParser
{
    /// <summary>
    /// Parse a grammar from a file and convert it to a <see cref="WAG"/>.
    /// </summary>
    /// <param name="path">The path to the file to parse.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">The file path was not valid.</exception>
    public static WAG Parse(string path)
    {
        if (!Path.Exists(path))
            throw new FileNotFoundException($"Could not find the file: {path}");

        var inputStream = CharStreams.fromPath(path);
        var lexer = new WAGLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new WAGParser(tokens) { BuildParseTree = true };

        var listener = new NoWaWAGListener();
        var walker = new ParseTreeWalker();
        walker.Walk(listener, parser.wag());

        return listener.Grammar;
    }
}
