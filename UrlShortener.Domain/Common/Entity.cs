namespace UrlShortener.Domain.Common;

/// <summary>
/// Base class for all domain entities.
/// Provides a strongly typed identifier for each entity.
/// </summary>
/// <typeparam name="TId">Type of the entity identifier.</typeparam>
public abstract class Entity<TId>
    where  TId : notnull
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public TId Id { get; }
    
    /// <summary>
    /// Initializes a new entity with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    protected Entity(TId id)
    {
        Id = id;
    }
}
