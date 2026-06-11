namespace UrlShortener.Contracts.UrlAction;

public record UrlResponse(Guid Id, string ShortUrl, DateTimeOffset CreatedAtUtc);