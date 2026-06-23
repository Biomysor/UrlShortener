using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Authentication.Common;

public record AuthenticationResult(
    User User, 
    string Token);