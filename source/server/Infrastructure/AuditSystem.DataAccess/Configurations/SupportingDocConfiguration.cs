using AuditSystem.Domain.Entities.SupportingDocs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class SupportingDocConfiguration : IEntityTypeConfiguration<SupportingDoc>
{
    public void Configure(EntityTypeBuilder<SupportingDoc> builder)
    {
        builder.ToTable("SupportingDocs");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(d => d.FileSize)
               .IsRequired();

        builder.Property(d => d.DriveFileId)
               .IsRequired();

        builder.HasIndex(d => d.FileName);
    }
}
