using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using UrlShortener.Application.Authentication.Behavior;
using UrlShortener.Application.Authentication.Queries;

namespace UrlShortener.UnitTests.Auth.Behavior;

public class ValidationBehaviorTests
{
    // Use a concrete request/response that works with the (dynamic) cast in ValidationBehavior.
    // LoginQuery is IRequest<ErrorOr<AuthenticationResult>> and ErrorOr<T> is assignable from List<Error>.
    private readonly Mock<RequestHandlerDelegate<ErrorOr<string>>> _nextMock = new();

    [Fact]
    public async Task Handle_ShouldCallNext_WhenNoValidatorsProvided()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, ErrorOr<string>>(validators);
        var request = new TestRequest("value");

        _nextMock
            .Setup(n => n(It.IsAny<CancellationToken>()))
            .ReturnsAsync("success");

        // Act
        var result = await behavior.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("success");

        _nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallNext_WhenValidatorsProduceNoFailures()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
            .Returns(new ValidationResult());

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<string>>(
            new[] { validatorMock.Object });

        var request = new TestRequest("value");

        _nextMock
            .Setup(n => n(It.IsAny<CancellationToken>()))
            .ReturnsAsync("success");

        // Act
        var result = await behavior.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("success");

        _nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationErrors_WhenValidatorProducesFailures()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new("Name", "Name is required"),
            new("Email", "Email is invalid")
        };

        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
            .Returns(new ValidationResult(failures));

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<string>>(
            new[] { validatorMock.Object });

        var request = new TestRequest(string.Empty);

        // Act
        var result = await behavior.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
        result.Errors[0].Code.Should().Be("Name");
        result.Errors[0].Description.Should().Be("Name is required");
        result.Errors[1].Code.Should().Be("Email");
        result.Errors[1].Description.Should().Be("Email is invalid");

        _nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenSingleValidatorFails()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new("Password", "Password must be at least 8 characters")
        };

        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
            .Returns(new ValidationResult(failures));

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<string>>(
            new[] { validatorMock.Object });

        var request = new TestRequest("bad");

        // Act
        var result = await behavior.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Password");
        result.FirstError.Description.Should().Be("Password must be at least 8 characters");
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken_WhenCallingNext()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, ErrorOr<string>>(validators);
        var request = new TestRequest("value");
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        _nextMock
            .Setup(n => n(token))
            .ReturnsAsync("success");

        // Act
        var result = await behavior.Handle(request, _nextMock.Object, token);

        // Assert
        _nextMock.Verify(n => n(token), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAggregateFailures_FromMultipleValidators()
    {
        // Arrange
        var validator1Mock = new Mock<IValidator<TestRequest>>();
        validator1Mock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
            .Returns(new ValidationResult(new[] { new ValidationFailure("Field1", "Error1") }));

        var validator2Mock = new Mock<IValidator<TestRequest>>();
        validator2Mock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
            .Returns(new ValidationResult(new[] { new ValidationFailure("Field2", "Error2") }));

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<string>>(
            new[] { validator1Mock.Object, validator2Mock.Object });

        var request = new TestRequest(string.Empty);

        // Act
        var result = await behavior.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
        result.Errors.Select(e => e.Code).Should().Contain(new[] { "Field1", "Field2" });

        _nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>Minimal request type for testing the behavior in isolation.</summary>
    private record TestRequest(string Value) : IRequest<ErrorOr<string>>;
}