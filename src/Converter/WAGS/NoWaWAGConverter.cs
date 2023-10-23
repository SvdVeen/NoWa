using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter.WAGs;

/// <summary>
/// Converts a <see cref="WAG"/> to Chomsky Normal Form.
/// </summary>
public class NoWaWAGConverter : NoWaConverter<WAG>
{
    /// <inheritdoc/>
    public NoWaWAGConverter(ILogger logger) : base(logger)
    {
        _steps = new List<IConversionStep<WAG>>()
        {
            new EmptyStringStep(Logger),
            new UnitProductionsStep(Logger),
            new UnreachableSymbolsStep(Logger),
            new SeparateTerminalsStep(Logger),
            new SplitNonterminalsStep(Logger),
        };
    }
}
