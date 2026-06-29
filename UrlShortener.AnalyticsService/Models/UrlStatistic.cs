using System.ComponentModel.DataAnnotations;

namespace UrlShortener.AnalyticsService.Models;

public class UrlStatistic
{
    public Guid Id { get; init; }

    public Guid UrlId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string LongUrl { get; init; } = string.Empty;

    public int ClickCount { get; set; }

    public DateTimeOffset? LastRedirectedAtUtc { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}