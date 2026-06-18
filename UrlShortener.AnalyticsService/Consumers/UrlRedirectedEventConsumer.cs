using MassTransit;
using Microsoft.EntityFrameworkCore;
using UrlShortener.AnalyticsService.Models;
using UrlShortener.AnalyticsService.Persistance;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.AnalyticsService.Consumers;

/// <summary>
///     Consumes URL redirected events and updates URL redirect statistics.
/// </summary>
public class UrlRedirectedEventConsumer(AnalyticsDbContext dbContext)
    : IConsumer<UrlRedirectedEvent>
{
    private readonly AnalyticsDbContext _dbContext = dbContext;

    /// <summary>
    ///     Handles UrlRedirectedEvent messages.
    ///     Creates a new statistics record if it does not exist,
    ///     otherwise increments click count and updates last redirect date.
    /// </summary>
    /// <param name="context">Message consume context.</param>
    public async Task Consume(ConsumeContext<UrlRedirectedEvent> context)
    {
        var message = context.Message;

        var statistic = await _dbContext.UrlStatistics
            .FirstOrDefaultAsync(x => x.Code == message.Code);

        if (statistic is null)
        {
            statistic = new UrlStatistic
            {
                Id = Guid.NewGuid(),
                UrlId = message.UrlId,
                Code = message.Code,
                LongUrl = message.LongUrl,
                ClickCount = 0
            };

            await _dbContext.UrlStatistics.AddAsync(statistic);
        }

        statistic.ClickCount++;
        statistic.LastRedirectedAtUtc = message.RedirectedAtUtc;

        var click = new UrlClick
        {
            Id = Guid.NewGuid(),
            Code = message.Code,
            RedirectedAtUtc = message.RedirectedAtUtc,
            IpAddress = message.IpAddress,
            UserAgent = message.UserAgent
        };

        await _dbContext.UrlClicks.AddAsync(click);

        await _dbContext.SaveChangesAsync();
    }
}