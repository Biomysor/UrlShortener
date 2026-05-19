using MassTransit;
using UrlShortener.Application.Common.Interfaces.Services;

namespace UrlShortener.Infrastructure.Services;

public class MassTransitMessagePublisher(IPublishEndpoint publishEndpoint)
    : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : class
    {
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}