using AuditSystem.Domain.Entities.Observations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class ObservationConfiguration : IEntityTypeConfiguration<Observation>
{
    public void Configure(EntityTypeBuilder<Observation> builder)
    {
        builder.ToTable("Observations");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description)
            .IsRequired(true)
            .HasMaxLength(500);
        
        builder.Property(x => x.Recommendation)
            .HasMaxLength(500);

        builder.Property(x => x.ActionPlan)
            .IsRequired(true)
            .HasMaxLength(500);
            
        builder.Property(x => x.ActionDate);

        builder.HasOne(x => x.AuditProgram)
            .WithMany(x => x.Observations)
            .HasForeignKey(x => x.AuditProgramId)
            .IsRequired(true);
    }
}