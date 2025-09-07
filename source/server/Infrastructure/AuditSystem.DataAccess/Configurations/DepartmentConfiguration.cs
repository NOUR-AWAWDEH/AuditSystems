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
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.SubLocationId)
            .IsRequired();

        // Relationships
        builder.HasOne(d => d.SubLocation)
            .WithMany(sl => sl.Departments)
            .HasForeignKey(d => d.SubLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(d => d.SubLocationId);
        builder.HasIndex(d => d.Name);
    }
}
