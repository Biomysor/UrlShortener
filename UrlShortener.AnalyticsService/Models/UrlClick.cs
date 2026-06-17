namespace UrlShortener.AnalyticsService.Models;

public class UrlClick
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public DateTimeOffset RedirectedAtUtc { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }
}