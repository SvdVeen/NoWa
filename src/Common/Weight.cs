using System.Globalization;

namespace NoWa.Common;

/// <summary>
/// A weight in a WAG.
/// </summary>
public class Weight
{
    private string? _attributeValue = null;
    private double? _doubleValue = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Weight"/> class without a value.
    /// </summary>
    public Weight() { }

    /// <summary>
    /// Create a copy of a different weight.
    /// </summary>
    /// <param name="other">The weight to copy.</param>
    public Weight(Weight other)
    {
        _attributeValue = other._attributeValue;
        _doubleValue = other._doubleValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Weight"/> class with an attribute value.
    /// </summary>
    /// <param name="value">The value of the weight.</param>
    public Weight(string value) => Set(value);

    /// <summary>
    /// Initializes a new instance of the <see cref="Weight"/> class with a numeric value.
    /// </summary>
    /// <param name="value">The value of the weight.</param>
    public Weight(double value) => Set(value);

    /// <summary>
    /// Gets the value of the weight.
    /// </summary>
    /// <returns>The value of the weight.</returns>
    public object? Get()
    {
        if(_attributeValue != null)
        {
            return _attributeValue;
        }
        if (_doubleValue.HasValue)
        {
            return _doubleValue.Value;
        }
        return null;
    }

    /// <summary>
    /// Tries to get the attribute value of the weight.
    /// </summary>
    /// <param name="result">The attribute value of the weight. <see langword="null"/> if it has no attribute value.</param>
    /// <returns><see langword="true"/> if the weight has an attribute value, <see langword="false"/> otherwise.</returns>
    public bool GetAttribute(out string? result)
    {
        result = _attributeValue;
        return result != null;
    }

    /// <summary>
    /// Tries to get the double value of the weight.
    /// </summary>
    /// <param name="result">The double value of the weight. -1 if it has no double value.</param>
    /// <returns><see langword="true"/> if the weight has a double value, <see langword="false"/> otherwise.</returns>
    public bool GetDouble(out double result)
    {
        result = _doubleValue ?? -1D;
        return _doubleValue.HasValue;
    }

    /// <summary>
    /// Checks whether a string is formatted as a valid attribute.
    /// </summary>
    /// <param name="attribute">A string representing an attribute.</param>
    /// <returns><see langword="true"/> if the string is formatted as a valid attribute, <see langword="false"/> otherwise.</returns></returns>
    private static bool IsValidAttribute(string attribute)
    {
        return attribute.Length == 2 && (attribute[0] == '&' || attribute[0] == '$' || attribute[0] == '!') && char.IsLower(attribute[1]);
    }

    /// <summary>
    /// Sets the value of the weight to a given attribute.
    /// </summary>
    /// <param name="value">The attribute to set the weight to.</param>
    /// <exception cref="ArgumentException"><paramref name="value"/> is not formatted as a valid attribute.</exception>
    public void Set(string value)
    {
        if (!IsValidAttribute(value))
        {
            throw new ArgumentException("The value of a weight must be a valid attribute.", nameof(value));
        }
        _attributeValue = value;
        _doubleValue = null;
    }

    /// <summary>
    /// Sets the value of the weight to a given numeric value.
    /// </summary>
    /// <param name="value">The value to set the weight to.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
    public void Set(double value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value of a weight cannot be negative.");
        }
        _doubleValue = value;
        _attributeValue = null;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_attributeValue != null)
        {
            return _attributeValue;
        }
        if (_doubleValue.HasValue)
        {
            return _doubleValue.Value.ToString(CultureInfo.InvariantCulture);
        }
        return string.Empty;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Weight weight &&
               _attributeValue == weight._attributeValue &&
               _doubleValue == weight._doubleValue;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_attributeValue, _doubleValue);
    }

    public static bool operator ==(Weight? left, Weight? right)
    {
        return EqualityComparer<Weight>.Default.Equals(left, right);
    }

    public static bool operator !=(Weight? left, Weight? right)
    {
        return !(left == right);
    }
}
