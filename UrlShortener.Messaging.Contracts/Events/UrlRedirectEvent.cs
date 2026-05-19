namespace UrlShortener.Messaging.Contracts.Events;

public record UrlRedirectedEvent(
    Guid UrlId,
    string Code,
    string LongUrl,
    DateTime RedirectedAt,
    string? IpAddress,
    string? UserAgent);