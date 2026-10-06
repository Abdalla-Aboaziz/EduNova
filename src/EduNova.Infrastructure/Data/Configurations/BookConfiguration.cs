using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.HasIndex(b => b.Title);

        builder.HasMany(b => b.Pages)
            .WithOne(p => p.Book)
            .HasForeignKey(p => p.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}