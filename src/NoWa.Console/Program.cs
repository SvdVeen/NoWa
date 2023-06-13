using NoWa.Converter;

string validArguments = @" Usage: NoWa.exe <path>
    path: the path to the grammar file to parse.";


if (args.Length == 0)
{
    Console.WriteLine("Invalid arguments!");
    Console.WriteLine(validArguments);
    return 1;
}

if (args[0] == "-help")
{
    Console.WriteLine(validArguments);
    return 0;
}

string path = args[0];
if (!Path.Exists(path))
{
    Console.WriteLine($"Could not find file: {path}");
    return 1; // Could not find file
}

NoWaConverter.Convert(path);

return 0;
