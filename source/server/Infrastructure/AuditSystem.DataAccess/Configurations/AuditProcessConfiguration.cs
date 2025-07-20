using AuditSystem.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class AuditProcessConfiguration : IEntityTypeConfiguration<AuditProcess>
{
    public void Configure(EntityTypeBuilder<AuditProcess> builder)
    {
        // Primary Key
        builder.HasKey(ap => ap.Id);

        // Properties
        builder.Property(ap => ap.ProcessName)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(ap => ap.SubDepartmentId)
               .IsRequired();

        // Relationships
        builder.HasOne(ap => ap.SubDepartment)
               .WithMany()
               .HasForeignKey(ap => ap.SubDepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ap => ap.SubProcesses)
               .WithOne(sp => sp.AuditProcess)
               .HasForeignKey(sp => sp.AuditProcessId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}