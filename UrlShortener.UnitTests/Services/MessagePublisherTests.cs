using MassTransit;
using Moq;
using UrlShortener.Infrastructure.Services;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.UnitTests.Services;

public class MessagePublisherTests
{
    [Fact]
    public async Task PublishAsync_ShouldPublishMessage_UsingMassTransit()
    {
        // Arrange
        var publishEndpointMock = new Mock<IPublishEndpoint>();

        var publisher = new MassTransitMessagePublisher(
            publishEndpointMock.Object);

        var message = new UrlCreatedEvent(
            Guid.NewGuid(),
            "https://google.com",
            "abc123",
            "http://localhost:5018/r/abc123",
            DateTime.UtcNow);

        // Act
        await publisher.PublishAsync(message, CancellationToken.None);

        // Assert
        publishEndpointMock.Verify(x => x.Publish(
                message,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}