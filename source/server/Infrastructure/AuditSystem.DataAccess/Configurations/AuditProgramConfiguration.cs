using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Users;
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

        // Change this from WithOne to WithMany
        builder.HasOne(ap => ap.ExitControl)
            .WithMany(ec => ec.AuditPrograms) 
            .HasForeignKey(ap => ap.ExitControlId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ap => ap.ActionOwner)
            .WithMany(au => au.AuditPrograms)
            .HasForeignKey(ap => ap.ActionOwnerId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ap => ap.ControlDesign)
            .WithMany(cd => cd.AuditPrograms)
            .HasForeignKey(ap => ap.ControlDesignId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ap => ap.Observations)
            .WithOne(o => o.AuditProgram)
            .HasForeignKey(o => o.AuditProgramId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(ap => ap.ExitControlId);
        builder.HasIndex(ap => ap.ControlDesignId);
        builder.HasIndex(ap => ap.Description);
    }
}