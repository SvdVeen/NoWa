using NoWa.Common;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// </summary>
public class NoWaConverter
{
    /// <summary>
    /// Convert a given grammar to CNF.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void Convert(Grammar grammar)
    {
        Console.WriteLine(grammar.ToString());
    }
}
