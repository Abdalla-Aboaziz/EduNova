using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations;

public class BookPageConfiguration : IEntityTypeConfiguration<BookPage>
{
    public void Configure(EntityTypeBuilder<BookPage> builder)
    {
        builder.Property(p => p.Content).IsRequired();
        builder.HasIndex(p => new { p.BookId, p.PageNumber }).IsUnique();
    }
}