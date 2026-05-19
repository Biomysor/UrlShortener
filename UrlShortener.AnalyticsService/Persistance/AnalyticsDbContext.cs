using Microsoft.EntityFrameworkCore;
using UrlShortener.AnalyticsService.Models;

namespace UrlShortener.AnalyticsService.Persistance;

public class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
        : base(options)
    {
    }

    public DbSet<UrlStatistic> UrlStatistics { get; set; }

    public DbSet<UrlClick> UrlClicks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlStatistic>(builder =>
        {
            builder.ToTable("UrlStatistics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(x => x.LongUrl)
                .IsRequired()
                .HasMaxLength(2048);

            builder.HasIndex(x => x.Code)
                .IsUnique();
        });

        modelBuilder.Entity<UrlClick>(builder =>
        {
            builder.ToTable("UrlClicks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(32);
        });
    }
}