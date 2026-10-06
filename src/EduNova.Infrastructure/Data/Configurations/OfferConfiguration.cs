using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduNova.Infrastructure.Data.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            // One instructor teaches a subject once per semester.
            builder.HasIndex(x => new { x.SubjectId, x.InstructorId, x.SemesterId }).IsUnique();

            // Offers are meaningless without their subject — cascade.
            // Removing an instructor is blocked while offers reference them.
            builder.HasOne(x => x.Subject)
             .WithMany(s => s.Offers)
             .HasForeignKey(x => x.SubjectId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Instructor)
             .WithMany(i => i.Offers)
             .HasForeignKey(x => x.InstructorId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Semester)
             .WithMany()
             .HasForeignKey(x => x.SemesterId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
