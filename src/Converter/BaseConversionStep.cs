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
}
