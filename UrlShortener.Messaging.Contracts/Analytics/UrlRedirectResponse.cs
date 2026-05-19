namespace UrlShortener.Messaging.Contracts.Analytics;

public record UrlRedirectResponse (
    string Code,
    DateTime RedirectedAt,
    string? IpAddress,
    string? UserAgent);