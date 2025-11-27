using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class ExitControlConfiguration : IEntityTypeConfiguration<ExitControl>
{
    public void Configure(EntityTypeBuilder<ExitControl> builder)
    {
        // Table Name
        builder.ToTable("ExitControls");

        // Primary Key
        builder.HasKey(ec => ec.Id);

        // Properties
        builder.Property(ec => ec.Description)
            .IsRequired(true)
            .HasMaxLength(500);
        
        builder.Property(ec => ec.Inadequate)
            .IsRequired(true);

        // Relationship with Objective (Restrict delete to prevent cascade path)
        builder.HasOne(ec => ec.Objective)
            .WithMany(o => o.ExitControls)
            .HasForeignKey(ec => ec.ObjectiveId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);


        // Indexes
        builder.HasIndex(ec => ec.ObjectiveId);
        builder.HasIndex(ec => ec.Inadequate);
    }
}