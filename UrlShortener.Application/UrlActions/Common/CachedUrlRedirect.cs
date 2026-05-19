namespace UrlShortener.Application.UrlActions.Common;

public record CachedUrlRedirect(
    Guid UrlId,
    string Code,
    string LongUrl);