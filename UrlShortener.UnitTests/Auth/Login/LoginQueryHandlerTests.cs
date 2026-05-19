using FluentAssertions;
using Moq;
using UrlShortener.Application.Authentication.Queries;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.UnitTests.Auth.Login;

public class LoginQueryHandlerTests
{
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentials_WhenUserDoesNotExist()
    {
        // Arrange
        var query = new LoginQuery(
            "notfound@gmail.com",
            "Password1");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(query.Email))
            .ReturnsAsync((User?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.Authentication.InvalidCredentials.Code);

        _passwordHasherMock.Verify(
            x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);

        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<User>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentials_WhenPasswordIsWrong()
    {
        // Arrange
        var query = new LoginQuery(
            "misha@gmail.com",
            "WrongPassword1");

        var user = User.Create(
            "misha",
            "misha@gmail.com",
            "hashed-password");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(query.Email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyHashedPassword(query.Password, user.PasswordHash))
            .Returns(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.Authentication.InvalidCredentials.Code);

        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<User>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthenticationResult_WhenCredentialsAreValid()
    {
        // Arrange
        var query = new LoginQuery(
            "misha@gmail.com",
            "Password1");

        var user = User.Create(
            "misha",
            "misha@gmail.com",
            "hashed-password");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(query.Email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyHashedPassword(query.Password, user.PasswordHash))
            .Returns(true);

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(user))
            .Returns("jwt-token");

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        result.Value.user.Should().Be(user);
        result.Value.token.Should().Be("jwt-token");

        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(user),
            Times.Once);
    }

    private LoginQueryHandler CreateHandler()
    {
        return new LoginQueryHandler(
            _jwtTokenGeneratorMock.Object,
            _userRepositoryMock.Object,
            _passwordHasherMock.Object);
    }
}