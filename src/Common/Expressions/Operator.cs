namespace NoWa.Common.Expressions;

/// <summary>
/// Represents an operator used in an expression.
/// </summary>
public enum Operator
{
    /// <summary>
    /// The equals ('=') operator.
    /// </summary>
    Equals = 0,

    /// <summary>
    /// The plus ('+') operator.
    /// </summary>
    Plus = 1,

    /// <summary>
    /// The minus ('-') operator.
    /// </summary>
    Minus = 2
}

/// <summary>
/// Contains extension methods for the <see cref="Operator"/> enum.
/// </summary>
public static class OperatorExtensions
{
    /// <summary>
    /// Gets the character that corresponds to the operator.
    /// </summary>
    /// <param name="operator">The operator to get the character for.</param>
    /// <returns>The character corresponding to the operator.</returns>
    /// <exception cref="InvalidOperationException">The operator was not valid (should not occur).</exception>
    public static char GetOperatorChar(this Operator @operator)
    {
        return @operator switch
        {
            Operator.Equals => '=',
            Operator.Plus => '+',
            Operator.Minus => '-',
            _ => throw new InvalidOperationException("Operator is invalid."),// This should only ever happen if code is changed incorrectly.
        };
    }
}

/// <summary>
/// Contains methods for parsing an <see cref="Operator"/> from input.
/// </summary>
public static class OperatorParser
{
    /// <summary>
    /// Parses an operator from a single character.
    /// </summary>
    /// <param name="input">The character to parse.</param>
    /// <returns>The <see cref="Operator"/> corresponding to the input.</returns>
    /// <exception cref="ArgumentException">The input is not a valid operator.</exception>
    public static Operator ParseChar(char input)
    {
        return input switch
        {
            '=' => Operator.Equals,
            '+' => Operator.Plus,
            '-' => Operator.Minus,
            _ => throw new ArgumentException("Could not parse operator from input.", nameof(input)),
        };
    }

    /// <summary>
    /// Parses an operator from a string.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <returns>The <see cref="Operator"/> corresponding to the input.</returns>
    /// <exception cref="ArgumentException">The input is not a valid operator.</exception>
    public static Operator ParseString(string input)
    {
        if (input.Length != 1)
        {
            throw new ArgumentException("Could not parse operator from input.", nameof(input));
        }
        return ParseChar(input[0]);
    }
}
