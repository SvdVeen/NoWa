using System.Globalization;

namespace NoWa.Common.Expressions;

/// <summary>
/// Represents an expression used as a computational formula.
/// </summary>
public class Expression
{
    /// <summary>
    /// The attribute being manipulated by the expression.
    /// </summary>
    public string Attribute { get; }

    /// <summary>
    /// The operator used in the expression.
    /// </summary>
    public Operator Operator { get; }

    /// <summary>
    /// The value used in the expression.
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Expression"/> class.
    /// </summary>
    /// <param name="attribute">The attribute influenced by the expression.</param>
    /// <param name="operator">The operator used in the expression.</param>
    /// <param name="value">The value to manipulate the attribute by.</param>
    /// <exception cref="ArgumentException"></exception>
    public Expression(string attribute, Operator @operator, double value)
    {
        if (!IsValidAttribute(attribute))
        {
            throw new ArgumentException("The given attribute is not valid.", nameof(attribute));
        }
        Attribute = attribute;
        Operator = @operator;
        Value = value;
    }

    /// <summary>
    /// Checks if an attribute string is a valid attribute reference.
    /// </summary>
    /// <param name="attribute">The attribute string to check.</param>
    /// <returns><see langword="true"/> if the string is a valid attribute reference; <see langword="false"/> otherwise.</returns>
    private static bool IsValidAttribute(string attribute)
        => (attribute.Length == 2)
        && (attribute[0] == '&' || attribute[0] == '$' || attribute[0] == '!')
        && char.IsLower(attribute[1]);

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Attribute} {Operator.GetOperatorChar()} {Value.ToString(CultureInfo.InvariantCulture)}";
    }
}
