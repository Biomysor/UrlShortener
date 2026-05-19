namespace UrlShortener.Application.Common.Interfaces.Services;

/// <summary>
/// Provides abstraction for publishing integration events.
/// Application layer does not depend on RabbitMQ or another concrete message broker.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the configured message broker.
    /// </summary>
    /// <typeparam name="TMessage">Type of message.</typeparam>
    /// <param name="message">Message instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : class;
}