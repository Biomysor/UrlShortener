using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.UrlAggregate.ValueObjects;

/// <summary>
/// Strongly typed identifier for the Url entity.
/// Wraps a Guid value and provides value-based equality.
/// </summary>
public sealed class UrlId : ValueObject
{
    
    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }
    
    private UrlId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new unique UrlId.
    /// </summary>
    /// <returns>New UrlId instance with a generated Guid.</returns>
    public static UrlId CreateUnique()
    {
        return new UrlId(Guid.NewGuid());
    }
    
    /// <summary>
    /// Creates a UrlId from an existing Guid value.
    /// Used when restoring entities from the database.
    /// </summary>
    /// <param name="value">Existing Guid value.</param>
    /// <returns>UrlId instance.</returns>
    public static UrlId Create( Guid value )
    {
        return new UrlId(value);
    }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return  Value;
    }
}