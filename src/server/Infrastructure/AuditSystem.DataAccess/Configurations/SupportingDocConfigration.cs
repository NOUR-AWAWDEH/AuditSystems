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
        
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        // Configure properties
        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.FileSize)
            .IsRequired();

        builder.Property(x => x.URL)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DriveFileId)
            .IsRequired();

        // Configure relationship with User
        builder.HasOne(x => x.User)
            .WithMany() // If User has a collection of SupportingDocs, use: .WithMany(u => u.SupportingDocs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict) // Or Cascade if appropriate for your use case
            .IsRequired();

        // Optional: Add indexes for performance
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.DriveFileId)
            .IsUnique(); // Assuming DriveFileId should be unique
    }
}