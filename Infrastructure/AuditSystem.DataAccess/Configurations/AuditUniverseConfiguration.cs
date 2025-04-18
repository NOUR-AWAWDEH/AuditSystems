using AuditSystem.Domain.Entities;
using AuditSystem.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class AuditUniverseConfiguration : IEntityTypeConfiguration<AuditUniverse>
{
    public void Configure(EntityTypeBuilder<AuditUniverse> builder)
    {
        // Primary Key
        builder.HasKey(au => au.Id);

        // Properties
        builder.Property(au => au.BusinessObjective)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(au => au.IndustryUpdate)
               .HasMaxLength(500);

        builder.Property(au => au.CompanyUpdate)
               .HasMaxLength(500);

        builder.Property(au => au.IsFinancialQuantifiable)
               .IsRequired();

        builder.Property(au => au.IsSpecialProject)
               .IsRequired();

        // Relationship with SpecialProject (Restrict delete to prevent cascade path)
        builder.HasOne(au => au.SpecialProject)
               .WithOne(sp => sp.AuditUniverse)
               .HasForeignKey<SpecialProject>(sp => sp.AuditUniverseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
