using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.DataAccess.Configurations;

public class SubDepartmentConfiguration : IEntityTypeConfiguration<SubDepartment>
{
    public void Configure(EntityTypeBuilder<SubDepartment> builder)
    {
        // Table configuration
        builder.ToTable("SubDepartments");

        // Primary Key
        builder.HasKey(sd => sd.Id);

        // Required properties
        builder.Property(sd => sd.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sd => sd.DepartmentId)
            .IsRequired(true);

        builder.HasOne(sd => sd.Department)
            .WithMany(d => d.SubDepartments)
            .HasForeignKey(sd => sd.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<AuditProcess>()
            .WithOne(ap => ap.SubDepartment)
            .HasForeignKey(ap => ap.SubDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sd => sd.DepartmentId);
        builder.HasIndex(sd => sd.Name);
    }
}