using UrlShortener.Domain.UrlAggregate;

namespace UrlShortener.Application.UrlActions.Common;

public record UrlResult(
    long Id,
    string ShortUrl);