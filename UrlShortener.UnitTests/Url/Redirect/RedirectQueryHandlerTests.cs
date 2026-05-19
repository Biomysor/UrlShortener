using FluentAssertions;
using Moq;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Application.UrlActions.Queries.RedirectQueries;
using UrlShortener.Domain.UrlAggregate.Entity;

namespace UrlShortener.UnitTests.Url.Redirect;


public class RedirectQueryHandlerTests
{
    private readonly Mock<IUrlRepository> _repositoryMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<IMessagePublisher> _messagePublisherMock = new();

    [Fact]
    public async Task Handle_ShouldReturnCachedLongUrl_AndPublishRedirectEvent_WhenCacheHit()
    {
        // Arrange
        var cachedUrl = new CachedUrlRedirect(
            Guid.NewGuid(),
            "abc123",
            "https://google.com");

        _cacheServiceMock
            .Setup(x => x.GetAsync<CachedUrlRedirect>(
                "url:code:abc123",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedUrl);

        var handler = CreateHandler();

        var query = new RedirectQuery("abc123");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("https://google.com");

        _repositoryMock.Verify(x => x.GetCodeAsync(It.IsAny<string>()), Times.Never);

        _messagePublisherMock.Verify(x => x.PublishAsync(
            It.IsAny<object>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldGetUrlFromDatabase_SetCache_AndPublishEvent_WhenCacheMiss()
    {
        // Arrange
        var url = UrlShortener.Domain.UrlAggregate.Entity.Url.Create("https://example.com");
        url.SetCode("abc123");

        _cacheServiceMock
            .Setup(x => x.GetAsync<CachedUrlRedirect>(
                "url:code:abc123",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CachedUrlRedirect?)null);

        _repositoryMock
            .Setup(x => x.GetCodeAsync("abc123"))
            .ReturnsAsync(url);

        var handler = CreateHandler();

        var query = new RedirectQuery("abc123");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("https://example.com");

        _repositoryMock.Verify(x => x.GetCodeAsync("abc123"), Times.Once);

        _cacheServiceMock.Verify(x => x.SetAsync(
            "url:code:abc123",
            It.IsAny<CachedUrlRedirect>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _messagePublisherMock.Verify(x => x.PublishAsync(
            It.IsAny<object>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUrlDoesNotExist()
    {
        // Arrange
        _cacheServiceMock
            .Setup(x => x.GetAsync<CachedUrlRedirect>(
                "url:code:wrong",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CachedUrlRedirect?)null);

        _repositoryMock
            .Setup(x => x.GetCodeAsync("wrong"))
            .ReturnsAsync((UrlShortener.Domain.UrlAggregate.Entity.Url?)null);

        var handler = CreateHandler();

        var query = new RedirectQuery("wrong");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("404");

        _messagePublisherMock.Verify(x => x.PublishAsync(
            It.IsAny<object>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    private RedirectQueryHandler CreateHandler()
    {
        return new RedirectQueryHandler(
            _repositoryMock.Object,
            _cacheServiceMock.Object,
            _messagePublisherMock.Object);
    }
}