using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.AnalyticsService.Persistance;
using UrlShortener.Messaging.Contracts.Analytics;

namespace UrlShortener.AnalyticsService.Controllers;


[ApiController]
[Route("analytics")]
public class AnalyticsController(AnalyticsDbContext dbContext) : ControllerBase
{
    private readonly AnalyticsDbContext _dbContext = dbContext;

    [HttpGet("{code}")]
    public async Task<IActionResult> GetStatistics(string code)
    {
        var statistic = await _dbContext.UrlStatistics
            .FirstOrDefaultAsync(x => x.Code == code);

        if (statistic is null)
        {
            return NotFound();
        }

        return Ok(new UrlStatisticsResponse(
            statistic.Code,
            statistic.LongUrl,
            statistic.ClickCount,
            statistic.LastRedirectedAt));
    }
}