using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class ObjectiveConfiguration : IEntityTypeConfiguration<Objective>
{
public void Configure(EntityTypeBuilder<Objective> builder)
{
       // Table configuration
       builder.ToTable("Objectives");

       // Primary Key
       builder.HasKey(rcm => rcm.Id);

       // Properties
       builder.Property(rcm => rcm.RiskControlMatrixId)
              .IsRequired(true);

       builder.Property(rcm => rcm.Description)
              .IsRequired(true)
              .HasMaxLength(500);

       // Relationships
       builder.HasOne(rcm => rcm.RiskControlMatrix)
              .WithMany(rcm => rcm.Objectives)
              .HasForeignKey(rcm => rcm.RiskControlMatrixId)
              .OnDelete(DeleteBehavior.Restrict);

       builder.HasMany(rcm => rcm.Risks)
              .WithOne(r => r.Objective)
              .HasForeignKey(r => r.ObjectiveId)
              .OnDelete(DeleteBehavior.Restrict);

       // Indexes for performance
       builder.HasIndex(rcm => rcm.RiskControlMatrixId);
       builder.HasIndex(rcm => rcm.Description);
}
}
