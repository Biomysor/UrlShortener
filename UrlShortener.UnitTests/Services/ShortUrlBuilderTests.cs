using FluentAssertions;
using Microsoft.AspNetCore.Http;
using UrlShortener.Infrastructure.UrlActions;

namespace UrlShortener.UnitTests.Services;

public class ShortUrlBuilderTests
{
    [Fact]
    public void BuildShortUrl_ShouldBuildUrl_WithSchemeHostAndCode()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Scheme = "http",
                Host = new HostString("localhost:5018")
            }
        };

        var accessor = new HttpContextAccessor
        {
            HttpContext = httpContext
        };

        var builder = new ShortUrlBuilder(accessor);

        // Act
        var result = builder.BuildShortUrl("abc123");

        // Assert
        result.Should().Be("http://localhost:5018/abc123");
    }
}