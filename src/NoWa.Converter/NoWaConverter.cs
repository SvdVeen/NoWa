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
        Start(grammar);
        Console.WriteLine(grammar.ToString());
    }

    /// <summary>
    /// Replaces the start rule with a new one.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    private static void Start(Grammar grammar)
    {
        Rule rule = new(new Nonterminal("NoWaSTART"), new Expression(grammar.GetRule(0).Nonterminal));
        grammar.InsertRule(0, rule);
    }
}
