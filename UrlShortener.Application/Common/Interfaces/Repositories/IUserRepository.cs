using UrlShortener.Application.Authentication.Common;
using UrlShortener.Domain.UserAggregate;

namespace UrlShortener.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    User GetByEmail(string email);
    void Add(User user);
}