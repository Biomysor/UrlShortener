using FluentAssertions;

namespace UrlShortener.UnitTests.Domain;
using UrlShortener.Domain.UrlAggregate.Entity;

public class UrlTests
{
    [Fact]
    public void Create_ShouldCreateUrl_WithEmptyCode()
    {
        // Arrange
        const string longUrl = "https://google.com";

        // Act
        var url = Url.Create(longUrl);

        // Assert
        url.Id.Should().NotBeNull();
        url.Id.Value.Should().NotBeEmpty();
        url.LongUrl.Should().Be(longUrl);
        url.Code.Should().BeEmpty();
        url.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SetCode_ShouldUpdateCode()
    {
        // Arrange
        var url = Url.Create("https://google.com");

        // Act
        url.SetCode("abc123");

        // Assert
        url.Code.Should().Be("abc123");
    }
}