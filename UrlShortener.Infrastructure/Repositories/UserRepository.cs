using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.UserAggregate;

namespace UrlShortener.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> _users = new();
    
    public  User? GetByEmail(string email)
    {
        return _users.SingleOrDefault(u => u.Email == email);
    }

    public void Add(User user)
    {
        _users.Add(user);
    }
}