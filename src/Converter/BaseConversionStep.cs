using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// Base class for conversion steps.
/// </summary>
public abstract class BaseConversionStep : IConversionStep
{
    /// <inheritdoc/>
    public ILogger Logger { get; private set; }

    /// <summary>
    /// Create a new instance of the step using the given logger.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use in the step.</param>
    public BaseConversionStep(ILogger logger) => Logger = logger;

    /// <inheritdoc/>
    public abstract void Convert(CFG grammar);

    /// <summary>
    /// Contains statistics about grammars and lets the step log them easily.
    /// </summary>
    protected class GrammarStats
    {
        /// <summary>
        /// Gets the number of terminals in the original grammar.
        /// </summary>
        public int Terminals { get; }

        /// <summary>
        /// Gets the number of nonterminals in the original grammar.
        /// </summary>
        public int Nonterminals { get; }

        /// <summary>
        /// Gets the number of productions in the original grammar.
        /// </summary>
        public int Productions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarStats"/> class.
        /// </summary>
        /// <param name="grammar">The grammar to save statistics for in this object.</param>
        public GrammarStats(CFG grammar)
        {
            Terminals = grammar.Terminals.Count;
            Nonterminals = grammar.Nonterminals.Count;
            Productions = grammar.Productions.Count;
        }

        /// <summary>
        /// Logs the difference between these statistics and a given grammar's statistics.
        /// </summary>
        /// <param name="grammar">The grammar to compare to the statistics.</param>
        /// <param name="logger">The logger to log the difference with.</param>
        /// <param name="level">The log level to use when logging the difference.</param>
        public void LogDiff(CFG grammar, ILogger logger, LogLevel level = LogLevel.Info)
        {
            if (Terminals != grammar.Terminals.Count)
            {
                logger.Log(level, $"\t{DiffWord(Terminals, grammar.Terminals.Count)} {Math.Abs(Terminals - grammar.Terminals.Count)} terminals.");
            }
            if (Nonterminals != grammar.Nonterminals.Count)
            {
                logger.Log(level, $"\t{DiffWord(Nonterminals, grammar.Nonterminals.Count)} {Math.Abs(Nonterminals - grammar.Nonterminals.Count)} nonterminals.");
            }
            if (Productions != grammar.Productions.Count)
            {
                logger.Log(level, $"\t{DiffWord(Productions, grammar.Productions.Count)} {Math.Abs(Productions - grammar.Productions.Count)} productions.");
            }
        }

        /// <summary>
        /// Gets the appropriate word for the difference between two values.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>"Added" if <paramref name="a"/> is less than <paramref name="b"/>, "Removed" otherwise.</returns>
        private static string DiffWord(int a, int b) => a < b ? "Added" : "Removed";
    }
}
