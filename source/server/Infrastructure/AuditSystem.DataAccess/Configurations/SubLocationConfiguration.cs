using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.DataAccess.Configurations;

public class SubLocationConfiguration : IEntityTypeConfiguration<SubLocation>
{
    public void Configure(EntityTypeBuilder<SubLocation> builder)
    {
        // Table configuration
        builder.ToTable("SubLocations");

        // Primary Key
        builder.HasKey(sl => sl.Id);

        // Required properties
        builder.Property(sl => sl.Name)
            .IsRequired(true)
            .HasMaxLength(100);

        builder.Property(sl => sl.Address)
            .IsRequired(true)
            .HasMaxLength(500);

        builder.Property(sl => sl.LocationId)
            .IsRequired(true);

        // Relationships
        builder.HasOne(sl => sl.Location)
            .WithMany(l => l.SubLocations)
            .HasForeignKey(sl => sl.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sl => sl.Departments)
            .WithOne(d => d.SubLocation)
            .HasForeignKey(d => d.SubLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(sl => sl.LocationId);
        builder.HasIndex(sl => sl.Name);
    }
}