using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UploadedFileConfigration : IEntityTypeConfiguration<UploadedFiles>
{
    public void Configure(EntityTypeBuilder<UploadedFiles> builder)
    {
        builder.ToTable("UploadedFiles");
        builder.Property(x => x.FileName).HasMaxLength(250);
        builder.Property(x => x.StoredFileName).HasMaxLength(250);
        builder.Property(x => x.ContentType).HasMaxLength(50);
        builder.Property(x => x.FileExtension).HasMaxLength(10);
    }
}
