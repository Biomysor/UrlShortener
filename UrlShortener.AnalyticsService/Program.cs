using MassTransit;
using Microsoft.EntityFrameworkCore;
using UrlShortener.AnalyticsService.Consumers;
using UrlShortener.AnalyticsService.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AnalyticsDatabase"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UrlRedirectedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });

        cfg.ReceiveEndpoint("url-redirected-event-queue", e =>
        {
            e.ConfigureConsumer<UrlRedirectedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();