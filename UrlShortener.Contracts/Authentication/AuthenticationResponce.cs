using UrlShortener.Domain.UserAggregate.ValueObjects;

namespace UrlShortener.Contracts.Authentication;

public record AuthenticationResponce(
    Guid Id,
    string Login,
    string Email,
    string Token);