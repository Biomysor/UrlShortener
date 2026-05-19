using MassTransit;
using Microsoft.EntityFrameworkCore;
using UrlShortener.AnalyticsService.Models;
using UrlShortener.AnalyticsService.Persistance;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.AnalyticsService.Consumers;

public class UrlRedirectedEventConsumer(AnalyticsDbContext dbContext)
    : IConsumer<UrlRedirectedEvent>
{
    private readonly AnalyticsDbContext _dbContext = dbContext;

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
        statistic.LastRedirectedAt = message.RedirectedAt;

        var click = new UrlClick
        {
            Id = Guid.NewGuid(),
            Code = message.Code,
            RedirectedAt = message.RedirectedAt,
            IpAddress = message.IpAddress,
            UserAgent = message.UserAgent
        };

        await _dbContext.UrlClicks.AddAsync(click);

        await _dbContext.SaveChangesAsync();
    }
}