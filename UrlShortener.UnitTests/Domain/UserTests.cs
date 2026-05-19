using FluentAssertions;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Create_ShouldCreateUser_WithCorrectProperties()
    {
        // Arrange
        var login = "misha";
        var email = "misha@gmail.com";
        var passwordHash = "hashed-password";

        // Act
        var user = User.Create(login, email, passwordHash);

        // Assert
        user.Id.Should().NotBeNull();
        user.Id.Value.Should().NotBeEmpty();
        user.Login.Should().Be(login);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
    }
}