using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class ControlDesignConfiguration : IEntityTypeConfiguration<ControlDesign>
{
    public void Configure(EntityTypeBuilder<ControlDesign> builder)
    {
        builder.ToTable("ControlDesigns");

        builder.HasKey(controlDesign => controlDesign.Id);

        builder.Property(controlDesign => controlDesign.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(controlDesign => controlDesign.Inadequate)
            .IsRequired(true);

        builder.HasOne(cd => cd.ExitControl)
            .WithMany(ec => ec.ControlDesigns)
            .HasForeignKey(cd => cd.ExitControlId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(cd => cd.AuditPrograms)
            .WithOne(ap => ap.ControlDesign)
            .HasForeignKey(ap => ap.ControlDesignId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}