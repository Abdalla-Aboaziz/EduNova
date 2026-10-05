using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);

            builder.HasIndex(x => new { x.YearId, x.Name }).IsUnique();

            // Semesters belong to their year — removing a year removes them.
            builder.HasOne(x => x.Year)
             .WithMany(y => y.Semesters)
             .HasForeignKey(x => x.YearId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
