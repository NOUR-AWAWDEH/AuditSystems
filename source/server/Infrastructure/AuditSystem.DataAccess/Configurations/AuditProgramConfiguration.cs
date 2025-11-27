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

        // Primary Key
        builder.HasKey(ap => ap.Id);

        // Properties
        builder.Property(ap => ap.Description)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(ap => ap.ExitControlId)
               .IsRequired();

        builder.Property(ap => ap.ActionOwnerId)
               .IsRequired();

        builder.Property(ap => ap.ControlDesignId)
               .IsRequired(false);

        // Relationships

        // ExitControl (required)
        builder.HasOne(ap => ap.ExitControl)
               .WithMany(ec => ec.AuditPrograms)
               .HasForeignKey(ap => ap.ExitControlId)
               .OnDelete(DeleteBehavior.Restrict);

        // ActionOwner (required) -> AppUser
        builder.HasOne(ap => ap.ActionOwner)
               .WithMany(au => au.OwnedAuditPrograms)
               .HasForeignKey(ap => ap.ActionOwnerId)
               .OnDelete(DeleteBehavior.Restrict);

        // ControlDesign (optional)
        builder.HasOne(ap => ap.ControlDesign)
               .WithMany(cd => cd.AuditPrograms)
               .HasForeignKey(ap => ap.ControlDesignId)
               .OnDelete(DeleteBehavior.Restrict);

        // Observations (collection)
        builder.HasMany(ap => ap.Observations)
               .WithOne(o => o.AuditProgram)
               .HasForeignKey(o => o.AuditProgramId)
               .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ap => ap.ExitControlId);
        builder.HasIndex(ap => ap.ActionOwnerId);
        builder.HasIndex(ap => ap.ControlDesignId);
        builder.HasIndex(ap => ap.Description);
    }
}
