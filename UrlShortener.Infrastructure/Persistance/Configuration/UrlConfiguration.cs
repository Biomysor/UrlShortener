using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.UrlAggregate.Entity;
using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Infrastructure.Persistance.Configuration;

public class UrlConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.ToTable("Urls");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => UrlId.Create(value))
            .ValueGeneratedNever();

        builder.Property(x => x.LongUrl)
            .HasMaxLength(1024);

        builder.Property(x => x.Code);

        builder.Property(x => x.CreatedAtUtc);

        builder.HasIndex(x => x.LongUrl)
            .IsUnique();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}