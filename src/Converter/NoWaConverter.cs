using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// 
/// <para>Currently only works for grammars without weighted attributes.</para>
/// </summary>
public class NoWaConverter
{
    /// <summary>
    /// The steps used in the conversion.
    /// </summary>
    private readonly IList<IConversionStep> _steps;

    /// <summary>
    /// The logger used by the converter.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Create a new instance of the converter with the given <see cref="ILogger"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging </param>
    public NoWaConverter(ILogger logger)
    {
        Logger = logger;
        _steps = new List<IConversionStep>()
        {
            new EmptyStringStep(Logger),
            new UnitProductionsStep(Logger),
            new UnreachableSymbolsStep(Logger)
        };
    }

    /// <summary>
    /// Convert the given <see cref="Grammar"/> to Chomsky Normal Form.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <returns>The converted grammar.</returns>
    public Grammar? Convert(Grammar grammar, bool continueOnError = false)
    {
        Grammar result = grammar;
        Logger.LogInfo("Starting CNF conversion...");
        Logger.LogDebug($"Initial grammar:{Environment.NewLine}{result}");

        for (int i = 0; i < _steps.Count; i++)
        {
            Logger.LogInfo($"Step {i + 1} of {_steps.Count}");
            try
            {
                _steps[i].Convert(grammar);
                Logger.LogDebug($"Intermediate grammar:{Environment.NewLine}{result}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Conversion encountered an unexpected error:{Environment.NewLine}\t{ex.GetType().Name} - {ex.Message}");
                if (!continueOnError)
                {
                    return null;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Eliminates nonsolitary terminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void SeparateTerminals(Grammar grammar)
    {
        Console.WriteLine("Separating terminals...");
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            foreach (var expr in rule.Productions.Where(expr => expr.Count > 1))
            {
                for (int j = 0; j < expr.Count; j++)
                {
                    if (expr[j] is Terminal terminal)
                    {
                        // A nonsolitary terminal is replaced with a nonterminal across the entire grammar.
                        Nonterminal nonterminal = grammar.AddNonterminal($"TERM-{terminal.Value.Replace(" ", "-")}");
                        grammar.ReplaceSymbol(terminal, nonterminal, false); // Keep the original because we will insert it again in the new rule.
                        // A new rule is added for the new nonterminal that refers to the old terminal.
                        Rule newRule = new(nonterminal);
                        newRule.AddProduction(terminal);
                        grammar.AddRule(newRule);
                    }
                }
            }
        }
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }

    /// <summary>
    /// Splits rules with more than two nonterminals.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    public static void ReduceNonterminals(Grammar grammar)
    {
        Console.WriteLine("Reducing nonterminals...");
        for (int i = 0, count = grammar.RuleCount; i < count; i++)
        {
            Rule rule = grammar.GetRule(i);
            foreach (var expr in rule.Productions.Where(expr => expr.OfType<Nonterminal>().Count() > 2))
            {
                for (int j = expr.Count - 1;  j >= 2; j--)
                {
                    Nonterminal nonterminal = grammar.AddNonterminal($"{expr[j - 1].Value}-{expr[j].Value}");
                    Rule newRule = new(nonterminal);
                    newRule.AddProduction(expr[j-1], expr[j]);
                    grammar.AddRule(newRule);
                    expr.RemoveAt(j);
                    expr.RemoveAt(j - 1);
                    expr.Add(nonterminal);
                }
            }
        }
        Console.WriteLine(grammar.ToString());
        Console.WriteLine();
    }
}
