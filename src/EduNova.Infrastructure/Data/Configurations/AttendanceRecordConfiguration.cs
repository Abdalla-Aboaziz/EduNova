using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations;

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords");

        builder.Property(a => a.StudentId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(a => a.Instructor)
            .WithMany()
            .HasForeignKey(a => a.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.StudentId, a.AttendanceDate });
        builder.HasIndex(a => a.InstructorId);
    }
}
