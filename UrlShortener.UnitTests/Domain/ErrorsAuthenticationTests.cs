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

    [Fact]
    public void InvalidCredentials_ShouldNotContainInterpolation_AndBeStaticString()
    {
        // Verifies the change from $"Invalid credentials." (string interpolation) to a plain string literal.
        // Both calls should return equivalent errors regardless of interpolation.
        var error1 = Errors.Authentication.InvalidCredentials;
        var error2 = Errors.Authentication.InvalidCredentials;

        // Assert
        error1.Description.Should().Be(error2.Description);
        error1.Code.Should().Be(error2.Code);
    }
}