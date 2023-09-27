namespace NoWa.Common;

/// <summary>
/// Represents the empty string in a grammar.
/// </summary>
public class EmptyString : ISymbol
{
    private static readonly EmptyString _instance = new();

    /// <summary>
    /// Gets the singleton instance of the <see cref="EmptyString"/>.
    /// </summary>
    public static EmptyString Instance { get => _instance; }

    /// <summary>
    /// Creates a new instance of the <see cref="EmptyString"/> class.
    /// </summary>
    private EmptyString() { }

    /// <summary>
    /// The value of the symbol.
    /// <para>The value of the empty string is simply "''".</para>
    /// </summary>
    public string Value => "''";

    /// <inheritdoc/>
    public override string ToString() => Value;

    /// <inheritdoc/>
    /// <remarks>
    /// Since this class is a singleton, we only have to check that it is an instance of <see cref="EmptyString"/>.
    /// </remarks>
    public override bool Equals(object? obj) => obj is EmptyString;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(EmptyString? left, EmptyString? right)
    {
        return EqualityComparer<EmptyString>.Default.Equals(left, right);
    }

    public static bool operator !=(EmptyString? left, EmptyString? right)
    {
        return !(left == right);
    }
}
