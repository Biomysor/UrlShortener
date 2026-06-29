using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}