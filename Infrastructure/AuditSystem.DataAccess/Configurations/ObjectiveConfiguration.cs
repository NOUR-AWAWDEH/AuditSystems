using AuditSystem.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations
{
    public class ObjectiveConfiguration : IEntityTypeConfiguration<Objective>
    {
        public void Configure(EntityTypeBuilder<Objective> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            // Relationships (Avoid Cascade Paths)
            builder.HasOne(o => o.RiskControlMatrix)
                   .WithMany()
                   .HasForeignKey(o => o.RiskControlMatrixId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Rating)
                   .WithMany()
                   .HasForeignKey(o => o.RatingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
