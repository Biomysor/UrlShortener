using ErrorOr;
namespace UrlShortener.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail(string email) => Error.Conflict(
            code: "User.DuplicateEmail",
            description: $"The email '{email}' is already in use.");
    }
}