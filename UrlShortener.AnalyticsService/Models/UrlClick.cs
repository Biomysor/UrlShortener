namespace UrlShortener.AnalyticsService.Models;

public class UrlClick
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public DateTime RedirectedAtUtc { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }
}