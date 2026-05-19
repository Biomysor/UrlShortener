namespace UrlShortener.Messaging.Contracts.Events;

public record UrlCreatedEvent(
    Guid UrlId,
    string Code,
    string LongUrl,
    string ShortUrl,
    DateTime CreatedAt);