using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class RiskConfiguration : IEntityTypeConfiguration<Risk>
{
    public void Configure(EntityTypeBuilder<Risk> builder)
    {
        // Primary Key
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.Description)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(r => r.RiskName)
                .IsRequired()
                .HasMaxLength(200);

        builder.Property(r => r.RateId)
                .IsRequired();

        builder.Property(r => r.ObjectiveId)
                .IsRequired();


        // Relationship with Objective
        builder.HasOne(r => r.Objective)
               .WithMany(r => r.Risks)
               .HasForeignKey(r => r.ObjectiveId)
               .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Rate
        builder.HasOne(r => r.Rate)
               .WithMany()
               .HasForeignKey(r => r.RateId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.ObjectiveId);
        builder.HasIndex(r => r.Description);
    }
}
