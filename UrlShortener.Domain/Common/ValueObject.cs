namespace UrlShortener.Domain.Common;

/// <summary>
/// Base class for value objects.
/// Value objects are compared by their values instead of object references.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the components that are used to compare value objects.
    /// </summary>
    /// <returns>Collection of equality components.</returns>
    public abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Determines whether the current value object is equal to another object.
    /// </summary>
    /// <param name="obj">Object to compare with.</param>
    /// <returns>True if objects are equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var valueObj = (ValueObject)obj;
        
        return GetEqualityComponents()
            .SequenceEqual(valueObj.GetEqualityComponents());
    }
    
    /// <summary>
    /// Compares two value objects for equality.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Compares two value objects for inequality.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Gets the hash code based on value object equality components.
    /// </summary>
    /// <returns>Calculated hash code.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}