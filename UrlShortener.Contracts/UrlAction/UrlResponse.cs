using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Contracts.UrlAction;

public record UrlResponse(Guid Id, string ShortUrl);