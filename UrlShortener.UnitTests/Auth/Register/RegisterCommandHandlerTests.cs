using FluentAssertions;
using Moq;
using UrlShortener.Application.Authentication.Commands.Register;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.UnitTests.Auth.Register;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    [Fact]
    public async Task Handle_ShouldReturnDuplicateEmail_WhenUserWithEmailAlreadyExists()
    {
        // Arrange
        var command = new RegisterCommand(
            "misha",
            "misha@gmail.com",
            "Password1");

        var existingUser = User.Create(
            "misha",
            "misha@gmail.com",
            "hashed-password");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, CancellationToken.None))
            .ReturnsAsync(existingUser);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.User.DuplicateEmail(command.Email).Code);

        _passwordHasherMock.Verify(
            x => x.HashPassword(It.IsAny<string>()),
            Times.Never);

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>(), CancellationToken.None),
            Times.Never);

        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<User>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldHashPassword_AddUser_AndReturnToken_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new RegisterCommand(
            "misha",
            "misha@gmail.com",
            "Password1");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, CancellationToken.None))
            .ReturnsAsync((User?)null);

        _passwordHasherMock
            .Setup(x => x.HashPassword(command.Password))
            .Returns("hashed-password");

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns("jwt-token");

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        result.Value.Token.Should().Be("jwt-token");
        result.Value.User.Login.Should().Be(command.Login);
        result.Value.User.Email.Should().Be(command.Email);
        result.Value.User.PasswordHash.Should().Be("hashed-password");

        _passwordHasherMock.Verify(
            x => x.HashPassword(command.Password),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.Is<User>(u =>
                u.Login == command.Login &&
                u.Email == command.Email &&
                u.PasswordHash == "hashed-password"), CancellationToken.None),
            Times.Once);

        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<User>()),
            Times.Once);
    }

    private RegisterCommandHandler CreateHandler()
    {
        return new RegisterCommandHandler(
            _jwtTokenGeneratorMock.Object,
            _userRepositoryMock.Object,
            _passwordHasherMock.Object);
    }
}