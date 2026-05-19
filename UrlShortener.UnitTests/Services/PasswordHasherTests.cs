using FluentAssertions;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.UnitTests.Services;

public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_ShouldReturnHash_DifferentFromPlainPassword()
    {
        // Arrange
        var hasher = new PasswordHasher();
        var password = "Password1";

        // Act
        var hash = hasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(password);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
    {
        // Arrange
        var hasher = new PasswordHasher();
        var password = "Password1";
        var hash = hasher.HashPassword(password);

        // Act
        var isValid = hasher.VerifyHashedPassword(password, hash);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var hasher = new PasswordHasher();
        var hash = hasher.HashPassword("Password1");

        // Act
        var isValid = hasher.VerifyHashedPassword("WrongPassword1", hash);

        // Assert
        isValid.Should().BeFalse();
    }
}