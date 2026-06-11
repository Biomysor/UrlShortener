using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.AnalyticsService.Persistance;
using UrlShortener.Messaging.Contracts.Analytics;

namespace UrlShortener.AnalyticsService.Controllers;

/// <summary>
/// Provides endpoints for reading URL redirect analytics.
/// </summary>
[ApiController]
[Route("analytics")]
public class AnalyticsController(AnalyticsDbContext dbContext) : ControllerBase
{
    private readonly AnalyticsDbContext _dbContext = dbContext;

    /// <summary>
    /// Gets redirect statistics for a short URL code.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>
    /// URL statistics if found; otherwise 404 Not Found.
    /// </returns>
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
            statistic.LastRedirectedAtUtc));
    }
}