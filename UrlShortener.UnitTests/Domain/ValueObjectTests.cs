using FluentAssertions;
using UrlShortener.Domain.UrlAggregate.ValueObjects;
using UrlShortener.Domain.UserAggregate.ValueObjects;

namespace UrlShortener.UnitTests.Domain;

public class ValueObjectTests
{
    [Fact]
    public void UrlId_ShouldBeEqual_WhenValuesAreEqual()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var id1 = UrlId.Create(guid);
        var id2 = UrlId.Create(guid);

        // Assert
        id1.Should().Be(id2);
        (id1 == id2).Should().BeTrue();
    }

    [Fact]
    public void UrlId_ShouldNotBeEqual_WhenValuesAreDifferent()
    {
        // Arrange
        var id1 = UrlId.Create(Guid.NewGuid());
        var id2 = UrlId.Create(Guid.NewGuid());

        // Assert
        id1.Should().NotBe(id2);
        (id1 != id2).Should().BeTrue();
    }

    [Fact]
    public void UserId_ShouldBeEqual_WhenValuesAreEqual()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var id1 = UserId.Create(guid);
        var id2 = UserId.Create(guid);

        // Assert
        id1.Should().Be(id2);
        (id1 == id2).Should().BeTrue();
    }
}