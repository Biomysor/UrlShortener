namespace UrlShortener.Application.Common.Interfaces.Services;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : class;
}