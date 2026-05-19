namespace UrlShortener.Messaging.Contracts.Events;

/// <summary>
/// Integration event that is published when a new short URL is created.
/// Used by notification service to send Telegram notifications.
/// </summary>
/// <param name="UrlId">Identifier of the created URL.</param>
/// <param name="LongUrl">Original long URL.</param>
/// <param name="Code">Generated short code.</param>
/// <param name="ShortUrl">Full short URL.</param>
/// <param name="CreatedAt">Date and time when the URL was created.</param>
public record UrlCreatedEvent(
    Guid UrlId,
    string Code,
    string LongUrl,
    string ShortUrl,
    DateTime CreatedAt);