using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Table configuration
        builder.ToTable("Departments");

        // Primary Key
        builder.HasKey(d => d.Id);

        // Required properties
        builder.Property(d => d.Name)
            .IsRequired(true)
            .HasMaxLength(200);

        builder.Property(d => d.LocationId)
            .IsRequired(true);

        // Relationships
        builder.HasOne(d => d.Location)
            .WithMany(l => l.Departments)
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(d => d.LocationId);
        builder.HasIndex(d => d.Name);
    }
}
