using ErrorOr;
using FluentAssertions;
using Moq;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Queries.UrlQueries;

namespace UrlShortener.UnitTests.Url.Query;

public class UrlQueryHandlerTests
{
    private readonly Mock<IUrlRepository> _repositoryMock = new();
    private readonly Mock<IShortUrlBuilder> _shortUrlBuilderMock = new();

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUrlDoesNotExist()
    {
        // Arrange
        const string longUrl = "https://nonexistent.example.com";

        _repositoryMock
            .Setup(x => x.GetByLongUrlAsync(longUrl, CancellationToken.None))
            .ReturnsAsync((UrlShortener.Domain.UrlAggregate.Entity.Url?)null);

        var handler = CreateHandler();
        var query = new UrlQuery(longUrl);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Url.NotFound");
        result.FirstError.Description.Should().Be("Url not found");

        _shortUrlBuilderMock.Verify(
            x => x.BuildShortUrl(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnUrlResult_WhenUrlExists()
    {
        // Arrange
        const string longUrl = "https://google.com";
        const string code = "abc123";
        const string shortUrl = "http://localhost:5018/abc123";

        var url = UrlShortener.Domain.UrlAggregate.Entity.Url.Create(longUrl);
        url.SetCode(code);

        _repositoryMock
            .Setup(x => x.GetByLongUrlAsync(longUrl, CancellationToken.None))
            .ReturnsAsync(url);

        _shortUrlBuilderMock
            .Setup(x => x.BuildShortUrl(code))
            .Returns(shortUrl);

        var handler = CreateHandler();
        var query = new UrlQuery(longUrl);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.ShortUrl.Should().Be(shortUrl);
        result.Value.Id.Should().Be(url.Id.Value);

        _repositoryMock.Verify(
            x => x.GetByLongUrlAsync(longUrl, CancellationToken.None),
            Times.Once);

        _shortUrlBuilderMock.Verify(
            x => x.BuildShortUrl(code),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUseCodeFromUrl_WhenBuildingShortUrl()
    {
        // Arrange
        const string longUrl = "https://example.com";
        const string code = "xyz789";

        var url = UrlShortener.Domain.UrlAggregate.Entity.Url.Create(longUrl);
        url.SetCode(code);

        _repositoryMock
            .Setup(x => x.GetByLongUrlAsync(longUrl, CancellationToken.None))
            .ReturnsAsync(url);

        _shortUrlBuilderMock
            .Setup(x => x.BuildShortUrl(code))
            .Returns($"http://short.ly/{code}");

        var handler = CreateHandler();
        var query = new UrlQuery(longUrl);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.ShortUrl.Should().Be($"http://short.ly/{code}");

        _shortUrlBuilderMock.Verify(
            x => x.BuildShortUrl(code),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectCreatedAtUtc_WhenUrlExists()
    {
        // Arrange
        const string longUrl = "https://example.org";
        const string code = "ts0001";

        var url = UrlShortener.Domain.UrlAggregate.Entity.Url.Create(longUrl);
        url.SetCode(code);

        _repositoryMock
            .Setup(x => x.GetByLongUrlAsync(longUrl, CancellationToken.None))
            .ReturnsAsync(url);

        _shortUrlBuilderMock
            .Setup(x => x.BuildShortUrl(code))
            .Returns("http://short.ly/ts0001");

        var handler = CreateHandler();
        var query = new UrlQuery(longUrl);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.CreatedAtUtc.Should().BeCloseTo(url.CreatedAtUtc, TimeSpan.FromSeconds(1));
    }

    private UrlQueryHandler CreateHandler()
    {
        return new UrlQueryHandler(
            _repositoryMock.Object,
            _shortUrlBuilderMock.Object);
    }
}