namespace UrlShortener.AnalyticsService.Models;

public class UrlStatistic
{
    public Guid Id { get; set; }

    public Guid UrlId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string LongUrl { get; set; } = string.Empty;

    public int ClickCount { get; set; }

    public DateTimeOffset? LastRedirectedAtUtc { get; set; }
}