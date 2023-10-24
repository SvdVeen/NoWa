using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NoWa.Common;
using CFGLexer = NoWa.Parser.Generated.NoWaCFGLexer;
using CFGParser = NoWa.Parser.Generated.NoWaCFGParser;

namespace NoWa.Parser;

/// <summary>
/// Parses a grammar from a file and converts it to a <see cref="CFG"/>.
/// </summary>
public static class NoWaCFGParser
{
    /// <summary>
    /// Parse a grammar from a file and convert it to a <see cref="CFG"/>.
    /// </summary>
    /// <param name="path">The path to the file to parse.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">The file path was not valid.</exception>
    public static CFG Parse(string path)
    {
        if (!Path.Exists(path))
            throw new FileNotFoundException($"Could not find the file: {path}");

        var inputStream = CharStreams.fromPath(path);
        var lexer = new CFGLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CFGParser(tokens) { BuildParseTree = true };

        var listener = new NoWaCFGListener();
        var walker = new ParseTreeWalker();
        walker.Walk(listener, parser.cfg());

        return listener.Grammar;
    }
}
