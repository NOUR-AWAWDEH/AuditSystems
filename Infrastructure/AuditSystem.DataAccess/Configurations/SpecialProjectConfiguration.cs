using AuditSystem.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class SpecialProjectConfiguration : IEntityTypeConfiguration<SpecialProject>
{
    public void Configure(EntityTypeBuilder<SpecialProject> builder)
    {
        // Primary Key
        builder.HasKey(sp => sp.Id);

        // Properties
        builder.Property(sp => sp.ProjectName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(sp => sp.Description)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(sp => sp.StartDate)
               .IsRequired();

        builder.Property(sp => sp.EndDate);

        // Relationship with AuditUniverse (Restrict delete to prevent cascade path)
        builder.HasOne(sp => sp.AuditUniverse)
               .WithOne(au => au.SpecialProject)
               .HasForeignKey<SpecialProject>(sp => sp.AuditUniverseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
