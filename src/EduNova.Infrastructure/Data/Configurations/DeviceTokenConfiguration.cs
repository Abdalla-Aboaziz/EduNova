using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations;

public class DeviceTokenConfiguration : IEntityTypeConfiguration<DeviceToken>
{
    public void Configure(EntityTypeBuilder<DeviceToken> builder)
    {
        builder.ToTable("DeviceTokens");

        builder.Property(d => d.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(d => d.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.Platform)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(d => d.Token).IsUnique();
        builder.HasIndex(d => d.UserId);
    }
}
