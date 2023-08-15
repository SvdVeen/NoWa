using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A conversion step that eliminates all unit productions in a grammar.
/// </summary>
public sealed class UnitProductionsStep : BaseConversionStep
{
    /// <inheritdoc/>
    public UnitProductionsStep(ILogger logger) : base(logger) { }

    /// <summary>
    /// Eliminates all unit productions in the given <see cref="Grammar"/>.
    /// </summary>
    /// <inheritdoc/>
    public override void Convert(Grammar grammar)
    {
        throw new NotImplementedException();
    }
}
