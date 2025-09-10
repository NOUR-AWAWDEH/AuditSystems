using AuditSystem.Domain.Entities.SupportingDocs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class SupportingDocConfigration : IEntityTypeConfiguration<SupportingDoc>
{
    public void Configure(EntityTypeBuilder<SupportingDoc> builder)
    {
        // Set the table name
        builder.ToTable("SupportingDocs");

        // Configure primary key
        builder.HasKey(x => x.Id);
        
        
        // Configure properties
        builder.Property(x => x.FileName)
            .IsRequired(true)
            .HasMaxLength(255);

        builder.Property(x => x.FileSize)
            .IsRequired(true);

        builder.Property(x => x.DriveFileId)
            .IsRequired(true);

        // Configure relationship with User
        builder.HasOne(x => x.User)
            .WithMany(u => u.SupportingDocs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict) 
            .IsRequired();

        // Optional: Add indexes for performance
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.DriveFileId)
            .IsUnique();
    }
}