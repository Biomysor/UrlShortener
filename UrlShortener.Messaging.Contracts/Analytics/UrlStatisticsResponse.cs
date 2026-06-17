namespace UrlShortener.Messaging.Contracts.Analytics;

public record UrlStatisticsResponse(
    string Code,
    string LongUrl,
    int ClickCount,
    DateTimeOffset? LastRedirectedAtUtc);