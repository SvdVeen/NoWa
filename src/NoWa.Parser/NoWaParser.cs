using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GLexer = NoWa.Parser.Generated.NoWaLexer;
using GParser = NoWa.Parser.Generated.NoWaParser;

namespace NoWa.Parser;

public class NoWaParser
{
    public NoWaParser() {
    }

    public void Parse(string path)
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
    }
}
