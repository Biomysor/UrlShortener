namespace UrlShortener.AnalyticsService.Models;

public class UrlClick
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public DateTimeOffset RedirectedAtUtc { get; init; }

    public string? IpAddress { get; init; }

    public string? UserAgent { get; init; }
}