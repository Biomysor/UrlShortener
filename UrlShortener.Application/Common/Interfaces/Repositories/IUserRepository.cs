using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}