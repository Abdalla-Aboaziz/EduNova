using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            // One final grade per student per subject — retakes would need a
            // different shape (attempt history); revisit only if the Figma asks.
            builder.HasIndex(x => new { x.StudentId, x.SubjectId }).IsUnique();

            builder.Property(x => x.StudentId).HasMaxLength(450); // ASP.NET Identity key size, for the future FK
            builder.Property(x => x.Score).HasPrecision(5, 2);
            builder.Property(x => x.MaxScore).HasPrecision(5, 2);

            // Student records are never removed by cascade: a subject with
            // grades cannot be deleted, and retiring an offer keeps the grade
            // with its offer link set to null.
            builder.HasOne(x => x.Subject)
             .WithMany()
             .HasForeignKey(x => x.SubjectId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Offer)
             .WithMany()
             .HasForeignKey(x => x.OfferId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
