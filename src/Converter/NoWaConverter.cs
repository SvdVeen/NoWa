using NoWa.Common;
using NoWa.Common.Logging;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// 
/// <para>Currently only works for grammars without weighted attributes.</para>
/// </summary>
public abstract class NoWaConverter<TGrammar>
    where TGrammar : CFG
{
    /// <summary>
    /// The steps used in the conversion.
    /// </summary>
    protected IList<IConversionStep<TGrammar>> _steps;

    /// <summary>
    /// The logger used by the converter.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Creates a new instance of the converter with the given <see cref="ILogger"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging.</param>
    public NoWaConverter(ILogger logger)
    {
        Logger = logger;
        _steps = new List<IConversionStep<TGrammar>>();
    }

    /// <summary>
    /// Converts the given grammar to Chomsky Normal Form.
    /// </summary>
    /// <param name="grammar">The grammar to convert.</param>
    /// <returns>The converted grammar.</returns>
    public virtual CFG? Convert(TGrammar grammar, bool continueOnError = false)
    {
        CFG result = grammar;
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
                    Logger.LogInfo("Conversion failed!");
                    return null;
                }
            }
        }
        Logger.LogInfo($"Conversion completed!");
        return result;
    }
}
