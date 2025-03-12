using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations
{
    public class RiskControlConfiguration : IEntityTypeConfiguration<RiskControl>
    {
        public void Configure(EntityTypeBuilder<RiskControl> builder)
        {
            // Primary Key
            builder.HasKey(rc => rc.Id);

            // Properties
            builder.Property(rc => rc.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            // Relationship with Risk (Restrict delete to prevent cascade path)
            builder.HasOne(rc => rc.Risk)
                   .WithMany()
                   .HasForeignKey(rc => rc.RiskId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Rating (Optional, could also be NoAction)
            builder.HasOne(rc => rc.Rating)
                   .WithMany()
                   .HasForeignKey(rc => rc.RatingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
