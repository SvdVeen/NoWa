using Antlr4.Runtime.Misc;
using NoWa.Parser.Generated;

namespace NoWa.Parser;

internal class NoWaListener : NoWaParserBaseListener
{
    public override void EnterGrammar_([NotNull] Generated.NoWaParser.Grammar_Context context)
    {
        Console.WriteLine("Enter Grammar");
    }
}
