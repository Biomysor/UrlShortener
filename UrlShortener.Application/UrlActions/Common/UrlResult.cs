using UrlShortener.Domain.UrlAggregate;
using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Application.UrlActions.Common;

public record UrlResult(
    Guid Id,
    string ShortUrl);