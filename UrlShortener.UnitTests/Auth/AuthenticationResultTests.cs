using FluentAssertions;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.UnitTests.Auth;

public class AuthenticationResultTests
{
    [Fact]
    public void AuthenticationResult_ShouldExposeUser_ViaUserProperty()
    {
        // Arrange
        var user = User.Create("misha", "misha@gmail.com", "hashed-password");
        const string token = "jwt-token";

        // Act
        var result = new AuthenticationResult(user, token);

        // Assert
        result.User.Should().Be(user);
    }

    [Fact]
    public void AuthenticationResult_ShouldExposeToken_ViaTokenProperty()
    {
        // Arrange
        var user = User.Create("misha", "misha@gmail.com", "hashed-password");
        const string token = "jwt-token";

        // Act
        var result = new AuthenticationResult(user, token);

        // Assert
        result.Token.Should().Be(token);
    }

    [Fact]
    public void AuthenticationResult_ShouldBeRecordType_WithValueEquality()
    {
        // Arrange
        var user = User.Create("misha", "misha@gmail.com", "hashed-password");
        const string token = "jwt-token";

        var result1 = new AuthenticationResult(user, token);
        var result2 = new AuthenticationResult(user, token);

        // Assert - records with the same values should be equal
        result1.Should().Be(result2);
    }

    [Fact]
    public void AuthenticationResult_User_ShouldHaveCorrectProperties()
    {
        // Arrange
        const string login = "testuser";
        const string email = "testuser@example.com";
        const string passwordHash = "hash123";

        var user = User.Create(login, email, passwordHash);
        var result = new AuthenticationResult(user, "some-token");

        // Assert
        result.User.Login.Should().Be(login);
        result.User.Email.Should().Be(email);
        result.User.PasswordHash.Should().Be(passwordHash);
    }
}