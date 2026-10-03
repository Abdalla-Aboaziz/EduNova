using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class DownloadedLectureConfiguration : IEntityTypeConfiguration<DownloadedLecture>
    {
        public void Configure(EntityTypeBuilder<DownloadedLecture> b)
        {
            b.HasIndex(x => new { x.UserId, x.LectureId }).IsUnique();

            b.HasOne(x => x.Lecture)
             .WithMany(l => l.DownloadedLectures)
             .HasForeignKey(x => x.LectureId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
