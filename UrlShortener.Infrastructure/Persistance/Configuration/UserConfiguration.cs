using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.UserAggregate.Entity;
using UrlShortener.Domain.UserAggregate.ValueObjects;


namespace UrlShortener.Infrastructure.Persistance.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .ValueGeneratedNever();
        
        builder.Property(x => x.Login)
            .IsRequired()
            .HasMaxLength(48);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.HasIndex(x => x.Login)
            .IsUnique();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();

    }
}