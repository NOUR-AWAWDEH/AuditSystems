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

        builder.Property(x => x.Highlight)
            .IsRequired(true)
            .HasMaxLength(500);

        builder.Property(x => x.Background)
            .IsRequired(true)
            .HasMaxLength(2000);

        builder.Property(x => x.IsActive)
            .IsRequired(true);

        builder.Property(x => x.Note)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(x => x.Rate)
            .WithMany(x => x.Observations)
            .HasForeignKey(x => x.RateId)
            .IsRequired(true);

        builder.HasMany(x => x.Recommendations)
            .WithOne(x => x.Observation)
            .HasForeignKey(x => x.ObservationId)
            .IsRequired(true);

        builder.HasOne(x => x.AuditProgram)
            .WithMany(x => x.Observations)
            .HasForeignKey(x => x.AuditProgramId)
            .IsRequired(true);

        builder.HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);

    }
}