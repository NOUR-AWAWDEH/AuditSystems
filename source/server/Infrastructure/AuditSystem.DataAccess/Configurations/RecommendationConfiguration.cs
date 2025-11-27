namespace AuditSystem.DataAccess.Configurations;

using AuditSystem.Domain.Entities.Recommendations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        builder.ToTable("Recommendations");
        
        builder.Property(x => x.Description)
            .IsRequired(true)
            .HasMaxLength(500);
        
        builder.Property(x => x.Title)
            .IsRequired(true)
            .HasMaxLength(100);

        builder.Property(x => x.ObservationId)
            .IsRequired(true);

        builder.HasOne(x => x.Observation)
            .WithMany(x => x.Recommendations)
            .HasForeignKey(x => x.ObservationId)
            .IsRequired(true);

        builder.HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(x => x.ObservationId);
    }
}
