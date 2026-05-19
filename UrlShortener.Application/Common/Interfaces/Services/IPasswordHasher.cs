namespace UrlShortener.Application.Common.Interfaces.Services;

/// <summary>
/// Provides password hashing and verification functionality.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password.
    /// </summary>
    /// <param name="password">Plain text password.</param>
    /// <returns>Password hash.</returns>
    string HashPassword(string password);
    
    /// <summary>
    /// Verifies whether a plain text password matches the stored hash.
    /// </summary>
    /// <param name="password">Plain text password.</param>
    /// <param name="passwordHashed">Stored password hash.</param>
    /// <returns>True if password is valid; otherwise, false.</returns>
    bool VerifyHashedPassword(string password, string passwordHashed);
}