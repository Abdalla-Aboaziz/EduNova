using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.HasIndex(x => new { x.SubjectId, x.Order });
            builder.HasOne<UploadedFiles>().WithMany().HasForeignKey(x => x.VideoFileId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<UploadedFiles>().WithMany().HasForeignKey(x => x.ThumbnailFileId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
