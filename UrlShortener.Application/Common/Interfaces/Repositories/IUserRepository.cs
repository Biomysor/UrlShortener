using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}