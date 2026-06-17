using FluentAssertions;
using Moq;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Commands;
using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.UnitTests.Url.Shorten;

public class ShortenUrlCommandHandlerTests
{
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<IUrlCodeGenerator> _codeGeneratorMock = new();
    private readonly Mock<IMessagePublisher> _messagePublisherMock = new();
    private readonly Mock<IUrlRepository> _repositoryMock = new();
    private readonly Mock<IShortUrlBuilder> _shortUrlBuilderMock = new();

    [Fact]
    public async Task Handle_ShouldReturnExistingShortUrl_WhenLongUrlAlreadyExists()
    {
        // Arrange
        var longUrl = "https://google.com";
        var existingUrl = UrlShortener.Domain.UrlAggregate.Entity.Url.Create(longUrl);
        existingUrl.SetCode("abc123");

        _repositoryMock
            .Setup(x => x.GetByLongUrlAsync(longUrl, CancellationToken.None))
            .ReturnsAsync(existingUrl);

        _shortUrlBuilderMock
            .Setup(x => x.BuildShortUrl("abc123"))
            .Returns("http://localhost:5018/r/abc123");

        var handler = CreateHandler();

        var command = new ShortenUrlCommand(longUrl);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.ShortUrl.Should().Be("http://localhost:5018/r/abc123");

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<UrlShortener.Domain.UrlAggregate.Entity.Url>(), CancellationToken.None), Times.Never);
        _messagePublisherMock.Verify(
            x => x.PublishAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateUrl_SaveToDatabase_SetCache_AndPublishEvent_WhenUrlDoesNotExist()
    {
        // Arrange
        var longUrl = "https://example.com";

        _repositoryMock
            .Setup(x => x.GetByLongUrlAsync(longUrl, CancellationToken.None))
            .ReturnsAsync((UrlShortener.Domain.UrlAggregate.Entity.Url?)null);

        _codeGeneratorMock
            .Setup(x => x.GenerateCode(It.IsAny<UrlId>()))
            .Returns("abc123");

        _shortUrlBuilderMock
            .Setup(x => x.BuildShortUrl("abc123"))
            .Returns("http://localhost:5018/r/abc123");

        var handler = CreateHandler();

        var command = new ShortenUrlCommand(longUrl);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.ShortUrl.Should().Be("http://localhost:5018/r/abc123");

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.Is<UrlShortener.Domain.UrlAggregate.Entity.Url>(u => u.LongUrl == longUrl && u.Code == "abc123"), CancellationToken.None),
            Times.Once);

        _cacheServiceMock.Verify(x => x.SetAsync(
            "url:code:abc123",
            It.IsAny<object>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _messagePublisherMock.Verify(x => x.PublishAsync(
            It.IsAny<object>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private ShortenUrlCommandHandler CreateHandler()
    {
        return new ShortenUrlCommandHandler(
            _repositoryMock.Object,
            _codeGeneratorMock.Object,
            _shortUrlBuilderMock.Object,
            _cacheServiceMock.Object,
            _messagePublisherMock.Object);
    }
}