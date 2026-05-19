using UrlShortener.Domain.UserAggregate;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Authentication.Common;

public record AuthenticationResult(
    User user, 
    string token);