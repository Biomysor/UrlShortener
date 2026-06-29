using UrlShortener.Domain.Common;
using UrlShortener.Domain.UserAggregate.ValueObjects;

namespace UrlShortener.Domain.UserAggregate.Entity;

/// <summary>
/// Represents an application user.
/// Stores user identity data and password hash.
/// </summary>
public class User : Entity<UserId>
{
    /// <summary>
    /// Gets the user login.
    /// </summary>
    public string Login { get; private set; }
    
    /// <summary>
    /// Gets the user email address.
    /// </summary>
    public string Email { get; private set; }
    
    /// <summary>
    /// Gets the hashed user password.
    /// Plain passwords are not stored in the database.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Required by Entity Framework Core.
    /// </summary>
    private User(UserId id, string login, string email, string passwordHash)
        : base(id)
    {
        Login = login;
        Email = email;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Creates a new user with a unique identifier.
    /// </summary>
    /// <param name="login">User login.</param>
    /// <param name="email">User email.</param>
    /// <param name="passwordHash">Hashed password.</param>
    /// <returns>New User entity.</returns>
    public static User Create(string login, string email, string passwordHash)
    {
        return new User(UserId.CreateUnique(), login, email, passwordHash);
    }
}