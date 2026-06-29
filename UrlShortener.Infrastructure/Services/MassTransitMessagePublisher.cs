using MassTransit;
using UrlShortener.Application.Common.Interfaces.Services;

namespace UrlShortener.Infrastructure.Services;

/// <summary>
/// Message publisher implementation based on MassTransit.
/// Publishes integration events to RabbitMQ.
/// </summary>
public class MassTransitMessagePublisher(IPublishEndpoint publishEndpoint)
    : IMessagePublisher
{
    /// <inheritdoc />
    public async Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : class
    {
        await publishEndpoint.Publish(message, cancellationToken);
    }
}