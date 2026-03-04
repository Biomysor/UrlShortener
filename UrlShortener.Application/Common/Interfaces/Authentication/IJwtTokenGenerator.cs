using UrlShortener.Domain.UserAggregate;

namespace UrlShortener.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}