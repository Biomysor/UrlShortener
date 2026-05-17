namespace UrlShortener.Application.Common.Interfaces.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string passwordHashed);
}