using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class AuditProgramConfiguration : IEntityTypeConfiguration<AuditProgram>
{
    public void Configure(EntityTypeBuilder<AuditProgram> builder)
    {
        builder.ToTable("AuditPrograms");

        builder.HasKey(ap => ap.Id);

        builder.Property(ap => ap.Description)
            .IsRequired(true)
            .HasMaxLength(500);

        builder.HasOne(ap => ap.ExitControl)
            .WithOne(ap => ap.AuditProgram)
            .HasForeignKey<AuditProgram>(ap => ap.ExitControlId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ap => ap.ControlDesign)
            .WithOne(cd => cd.AuditProgram)
            .HasForeignKey<AuditProgram>(ap => ap.ControlDesignId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ap => ap.Observations)
            .WithOne(o => o.AuditProgram)
            .HasForeignKey(o => o.AuditProgramId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(ap => ap.ExitControlId);
        builder.HasIndex(ap => ap.ControlDesignId);
    }
}