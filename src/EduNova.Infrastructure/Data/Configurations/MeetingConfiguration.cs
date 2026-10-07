using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.ToTable("Meetings");

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Description)
                .HasMaxLength(1000); 

            builder.Property(m => m.JoinCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.RoomId)
                .HasMaxLength(255);

            builder.HasIndex(m => m.JoinCode).IsUnique();

            builder.HasMany(m => m.Participants)
                .WithOne(p => p.Meeting)
                .HasForeignKey(p => p.MeetingId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
