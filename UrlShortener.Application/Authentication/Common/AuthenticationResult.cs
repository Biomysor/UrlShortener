using UrlShortener.Domain.UserAggregate;

namespace UrlShortener.Application.Authentication.Common;

public record AuthenticationResult(
    User user, 
    string token);