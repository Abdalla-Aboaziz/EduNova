using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(200);
            builder.Property(x => x.AcademicTitle).HasMaxLength(50);
            builder.Property(x => x.Bio).HasMaxLength(2000);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.ImageUrl).HasMaxLength(500);
        }
    }
}
