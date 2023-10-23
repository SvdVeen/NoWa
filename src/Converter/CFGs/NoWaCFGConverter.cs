using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter.CFGs;

/// <summary>
/// Converts a <see cref="CFG"/> to Chomsky Normal Form.
/// </summary>
public class NoWaCFGConverter : NoWaConverter<CFG>
{
    /// <inheritdoc/>
    public NoWaCFGConverter(ILogger logger) : base(logger)
    {
        _steps = new List<IConversionStep<CFG>>()
        {
            new EmptyStringStep(Logger),
            new UnitProductionsStep(Logger),
            new UnreachableSymbolsStep(Logger),
            new SeparateTerminalsStep(Logger),
            new SplitNonterminalsStep(Logger),
        };
    }
}
