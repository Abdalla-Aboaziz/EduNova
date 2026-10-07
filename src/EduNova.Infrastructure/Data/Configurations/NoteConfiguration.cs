using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.Property(n => n.Title).IsRequired().HasMaxLength(150);
        builder.Property(n => n.Content).IsRequired();
        builder.Property(n => n.Color).IsRequired().HasMaxLength(9);

        //builder.HasOne(n => n.User)
          //  .WithMany(u => u.Notes)
           // .HasForeignKey(n => n.UserId)
            //.OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.UserId);
    }
}