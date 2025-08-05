using AuditSystem.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class SubProcessConfiguration : IEntityTypeConfiguration<SubProcess>
{
       public void Configure(EntityTypeBuilder<SubProcess> builder)
       {
              // Primary Key
              builder.HasKey(sp => sp.Id);

              // Properties
              builder.Property(sp => sp.Particular)
                     .IsRequired()
                     .HasMaxLength(500);

              builder.Property(sp => sp.AuditProcessId)
                     .IsRequired();

              // Relationships
              builder.HasOne(sp => sp.AuditProcess)
                     .WithMany(ap => ap.SubProcesses)
                     .HasForeignKey(sp => sp.AuditProcessId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasMany(sp => sp.RiskControlMatrices)
                  .WithOne(rcm => rcm.SubProcess)
                  .HasForeignKey(rcm => rcm.SubProcessId)
                  .OnDelete(DeleteBehavior.Restrict);

              builder.HasIndex(sp => sp.AuditProcessId);
              builder.HasIndex(sp => sp.Particular);
    }
}