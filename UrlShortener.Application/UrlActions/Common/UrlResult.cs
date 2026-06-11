namespace UrlShortener.Application.UrlActions.Common;

public record UrlResult(
    Guid Id,
    string ShortUrl,
    DateTimeOffset CreatedAtUtc);