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
        builder.Property(d => d.SubProcessId)
            .IsRequired(true);

        builder.Property(d => d.Description)
            .HasMaxLength(500)
            .IsRequired(true);

        // Relationships
        builder.HasOne(d => d.SubProcess)
            .WithMany(sp => sp.RiskControlMatrices)
            .HasForeignKey(d => d.SubProcessId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Objectives)
            .WithOne(o => o.RiskControlMatrix)
            .HasForeignKey(o => o.RiskControlMatrixId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(d => d.SubProcessId);
        builder.HasIndex(d => d.Description);
        
    }
}