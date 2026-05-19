using MassTransit;
using Telegram.Bot;
using UrlShortener.NotificationsService.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ITelegramBotClient>(_ =>
{
    var token = builder.Configuration["Telegram:BotToken"];

    if (string.IsNullOrWhiteSpace(token))
    {
        throw new InvalidOperationException("Telegram bot token is not configured.");
    }

    return new TelegramBotClient(token);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UrlCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });

        cfg.ReceiveEndpoint("url-created-event-queue", e =>
        {
            e.ConfigureConsumer<UrlCreatedEventConsumer>(context);
        });
    });
});

var host = builder.Build();

host.Run();