using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class RiskControlMatrixConfiguration : IEntityTypeConfiguration<RiskControlMatrix>
{
    public void Configure(EntityTypeBuilder<RiskControlMatrix> builder)
    {
        // Table configuration
        builder.ToTable("RiskControlMatrices");

        // Primary Key
        builder.HasKey(d => d.Id);

        // Properties
        builder.Property(d => d.AuditProcessId)
            .IsRequired(true);

        builder.Property(d => d.Description)
            .HasMaxLength(500)
            .IsRequired(true);

        // Relationships
        builder.HasOne(d => d.AuditProcess)
            .WithMany(ap => ap.RiskControlMatrices)
            .HasForeignKey(d => d.AuditProcessId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Objectives)
            .WithOne(o => o.RiskControlMatrix)
            .HasForeignKey(o => o.RiskControlMatrixId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);


        // Indexes for performance
        builder.HasIndex(d => d.AuditProcessId);
        builder.HasIndex(d => d.Description);
        
    }
}