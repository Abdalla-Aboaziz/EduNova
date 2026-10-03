using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class MyListItemConfiguration : IEntityTypeConfiguration<MyListItem>
    {
        public void Configure(EntityTypeBuilder<MyListItem> b)
        {
            b.HasIndex(x => new { x.UserId, x.LectureId }).IsUnique();

            b.HasOne(x => x.Lecture)
             .WithMany(l => l.MyListItems)
             .HasForeignKey(x => x.LectureId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
