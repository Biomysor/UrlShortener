namespace UrlShortener.Messaging.Contracts.Analytics;

public record UrlRedirectResponse(
    string Code,
    DateTimeOffset RedirectedAtUtc,
    string? IpAddress,
    string? UserAgent);