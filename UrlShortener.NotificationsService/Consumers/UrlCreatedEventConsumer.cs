using MassTransit;
using Telegram.Bot;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.NotificationsService.Consumers;

/// <summary>
/// Consumes URL created events and sends Telegram notifications.
/// </summary>
public class UrlCreatedEventConsumer(
    ITelegramBotClient telegramBotClient,
    IConfiguration configuration)
    : IConsumer<UrlCreatedEvent>
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Handles UrlCreatedEvent messages and sends notification to configured Telegram chat.
    /// </summary>
    /// <param name="context">Message consume context.</param>
    public async Task Consume(ConsumeContext<UrlCreatedEvent> context)
    {
        var message = context.Message;

        var chatId = _configuration["Telegram:ChatId"];

        if (string.IsNullOrWhiteSpace(chatId))
        {
            return;
        }

        var text = $"""
                    Створено нове коротке посилання

                    Long URL: {message.LongUrl}
                    Short URL: {message.ShortUrl}
                    Code: {message.Code}
                    Created at: {message.CreatedAt:yyyy-MM-dd HH:mm:ss}
                    """;

        await _telegramBotClient.SendMessage(
            chatId: chatId,
            text: text);
    }
}