using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.DataAccess.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        // Table configuration
        builder.ToTable("Locations");

        // Required properties
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.CompanyId)
            .IsRequired();

        builder.Property(l => l.UserId)
            .IsRequired();

        // Optional properties with constraints
        builder.Property(l => l.Address)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(l => l.Company)
            .WithMany(c => c.Locations)
            .HasForeignKey(l => l.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(l => l.CompanyId);
        builder.HasIndex(l => l.UserId);
        builder.HasIndex(l => l.Name);
    }
}