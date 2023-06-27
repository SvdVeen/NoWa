using NoWa.Common;
using NoWa.Converter;
using NoWa.Parser;

string helpInfo = @"Usage: NoWa.exe <path>
    path: the path to the grammar file to parse.

Converts the given grammer to Chomsky Normal Form.
The first rule in the grammar is assumed to be the start rule.";

Console.WriteLine("NoWa: a Chomsky Normal Form converter for Weighted Attribute Grammars.");
Console.WriteLine("06-2023, Suzanne van der Veen, University of Twente");
Console.WriteLine();

if (args.Length == 0)
{
    Console.WriteLine("Invalid arguments!");
    Console.WriteLine(helpInfo);
    return 1;
}

if (args[0] == "-help")
{
    Console.WriteLine(helpInfo);
    return 0;
}

string path = args[0];
if (!Path.Exists(path))
{
    Console.WriteLine($"Could not find file: {path}");
    return 1; // Could not find file
}

NoWaParser parser = new();
Grammar grammar = parser.Parse(path);
NoWaConverter.Convert(grammar);

return 0;
