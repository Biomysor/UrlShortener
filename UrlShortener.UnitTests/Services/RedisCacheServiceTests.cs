using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.UnitTests.Services;

public class RedisCacheServiceTests
{
    [Fact]
    public async Task SetAsync_AndGetAsync_ShouldReturnSavedObject()
    {
        // Arrange
        IDistributedCache memoryCache = new MemoryDistributedCache(
            Options.Create(new MemoryDistributedCacheOptions()));

        var cacheService = new CachingService(memoryCache);

        const string key = "url:code:abc123";
        var value = new TestCacheModel(
            Guid.NewGuid(),
            "abc123",
            "https://google.com");

        // Act
        await cacheService.SetAsync(
            key,
            value,
            TimeSpan.FromMinutes(5),
            CancellationToken.None);

        var result = await cacheService.GetAsync<TestCacheModel>(
            key,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UrlId.Should().Be(value.UrlId);
        result.Code.Should().Be(value.Code);
        result.LongUrl.Should().Be(value.LongUrl);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        IDistributedCache memoryCache = new MemoryDistributedCache(
            Options.Create(new MemoryDistributedCacheOptions()));

        var cacheService = new CachingService(memoryCache);

        // Act
        var result = await cacheService.GetAsync<TestCacheModel>(
            "not-existing-key",
            CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValueFromCache()
    {
        // Arrange
        IDistributedCache memoryCache = new MemoryDistributedCache(
            Options.Create(new MemoryDistributedCacheOptions()));

        var cacheService = new CachingService(memoryCache);

        const string key = "url:code:abc123";

        await cacheService.SetAsync(
            key,
            new TestCacheModel(Guid.NewGuid(), "abc123", "https://google.com"),
            TimeSpan.FromMinutes(5),
            CancellationToken.None);

        // Act
        await cacheService.RemoveAsync(key, CancellationToken.None);

        var result = await cacheService.GetAsync<TestCacheModel>(
            key,
            CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    private record TestCacheModel(
        Guid UrlId,
        string Code,
        string LongUrl);
}