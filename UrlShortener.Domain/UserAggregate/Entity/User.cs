using UrlShortener.Domain.Common;
using UrlShortener.Domain.UserAggregate.ValueObjects;

namespace UrlShortener.Domain.UserAggregate.Entity;

public class User : Entity<UserId>
{
    public string Login { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    private User(UserId id, string login, string email, string passwordHash)
        : base(id)
    {
        Login = login;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static User Create(string login, string email, string passwordHash)
    {
        return new User(UserId.CreateUnique(), login, email, passwordHash);
    }
}