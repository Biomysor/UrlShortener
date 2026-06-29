using ErrorOr;
using FluentAssertions;
using UrlShortener.Domain.Common.Errors;

namespace UrlShortener.UnitTests.Domain;

public class ErrorsAuthenticationTests
{
    [Fact]
    public void InvalidCredentials_ShouldHaveExpectedCode()
    {
        // Act
        var error = Errors.Authentication.InvalidCredentials;

        // Assert
        error.Code.Should().Be("Auth.InvalidCredential");
    }

    [Fact]
    public void InvalidCredentials_ShouldHaveExpectedDescription()
    {
        // Act
        var error = Errors.Authentication.InvalidCredentials;

        // Assert
        error.Description.Should().Be("Invalid credentials.");
    }

    [Fact]
    public void InvalidCredentials_ShouldBeValidationType()
    {
        // Act
        var error = Errors.Authentication.InvalidCredentials;

        // Assert
        error.Type.Should().Be(ErrorType.Validation);
    }
}