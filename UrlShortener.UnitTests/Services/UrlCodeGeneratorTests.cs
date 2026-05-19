using FluentAssertions;
using UrlShortener.Domain.UrlAggregate.ValueObjects;
using UrlShortener.Infrastructure.UrlActions;

namespace UrlShortener.UnitTests.Services;

public class UrlCodeGeneratorTests
{
    [Fact]
    public void GenerateCode_ShouldReturnNotEmptyCode()
    {
        // Arrange
        var generator = new UrlCodeGenerator();
        var id = UrlId.CreateUnique();

        // Act
        var code = generator.GenerateCode(id);

        // Assert
        code.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GenerateCode_ShouldReturnSameCode_ForSameId()
    {
        // Arrange
        var generator = new UrlCodeGenerator();
        var guid = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var id1 = UrlId.Create(guid);
        var id2 = UrlId.Create(guid);

        // Act
        var code1 = generator.GenerateCode(id1);
        var code2 = generator.GenerateCode(id2);

        // Assert
        code1.Should().Be(code2);
    }

    [Fact]
    public void GenerateCode_ShouldReturnDifferentCodes_ForDifferentIds()
    {
        // Arrange
        var generator = new UrlCodeGenerator();

        var id1 = UrlId.Create(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var id2 = UrlId.Create(Guid.Parse("22222222-2222-2222-2222-222222222222"));

        // Act
        var code1 = generator.GenerateCode(id1);
        var code2 = generator.GenerateCode(id2);

        // Assert
        code1.Should().NotBe(code2);
    }
}