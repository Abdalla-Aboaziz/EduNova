using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class MeetingParticipantConfiguration : IEntityTypeConfiguration<MeetingParticipant>
    {
        public void Configure(EntityTypeBuilder<MeetingParticipant> builder)
        {
            builder.ToTable("MeetingParticipants");

            builder.Property(mp => mp.UserId)
                .IsRequired()
                .HasMaxLength(450);

            // بنعمل Composite Unique Index عشان اليوزر الواحد ما يتسجلش مرتين في نفس الميتنج
            builder.HasIndex(mp => new { mp.MeetingId, mp.UserId }).IsUnique();
        }
    }
}
