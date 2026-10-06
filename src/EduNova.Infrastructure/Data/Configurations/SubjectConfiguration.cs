using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.Property(x => x.Code).HasMaxLength(20);
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);

            builder.HasIndex(x => x.Code).IsUnique();

            // Subjects are curriculum reference data — deleting a year or
            // semester that still has subjects is blocked, not cascaded.
            builder.HasOne(x => x.Year)
             .WithMany()
             .HasForeignKey(x => x.YearId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Semester)
             .WithMany()
             .HasForeignKey(x => x.SemesterId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
