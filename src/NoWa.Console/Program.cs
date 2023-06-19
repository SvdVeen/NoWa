using NoWa.Common;
using NoWa.Converter;
using NoWa.Parser;

string helpInfo = @" Usage: NoWa.exe <path>
    path: the path to the grammar file to parse.";


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

NoWaParser parser = new NoWaParser();
Grammar grammar = parser.Parse(path);
NoWaConverter.Convert(grammar);

return 0;
