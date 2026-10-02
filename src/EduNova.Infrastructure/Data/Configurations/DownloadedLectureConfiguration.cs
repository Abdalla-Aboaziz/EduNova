using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class DownloadedLectureConfiguration : IEntityTypeConfiguration<DownloadedLecture>
    {
        public void Configure(EntityTypeBuilder<DownloadedLecture> builder)
        {
           builder.HasIndex(x=>new {x.UserId,x.LectureId }).IsUnique();
            builder.HasOne<Lecture>()
                .WithMany()
                .HasForeignKey(x => x.LectureId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
