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
                        .IsRequired(true)
                        .HasMaxLength(500);

                builder.Property(r => r.RiskName)
                        .IsRequired(true)
                        .HasMaxLength(250);

                builder.Property(r => r.ObjectiveId)
                        .IsRequired(true);

                builder.HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);


                builder.HasIndex(r => r.ObjectiveId);
                builder.HasIndex(r => r.Description);
        }
}
