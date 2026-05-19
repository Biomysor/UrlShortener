namespace UrlShortener.Messaging.Contracts.Notifications;

public record TelegramNotificationMessage(
    string ChatId,
    string Message);