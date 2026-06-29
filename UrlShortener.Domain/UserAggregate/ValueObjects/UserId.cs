using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Strongly typed identifier for the User entity.
/// Wraps a Guid value and provides value-based equality.
/// </summary>
public class UserId : ValueObject
{
    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new unique UserId.
    /// </summary>
    /// <returns>New UserId instance with a generated Guid.</returns>
    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    /// <summary>
    /// Creates a UserId from an existing Guid value.
    /// Used when restoring entities from the database.
    /// </summary>
    /// <param name="value">Existing Guid value.</param>
    /// <returns>UserId instance.</returns>
    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return  Value;
    }
}