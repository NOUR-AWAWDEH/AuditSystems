using AuditSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class RateConfiguration : IEntityTypeConfiguration<Rate>
{
    public void Configure(EntityTypeBuilder<Rate> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Level)
            .IsRequired(true)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(r => r.Level).IsUnique();

        // Seed 5 default ratings with fixed GUIDs and constant dates
        builder.HasData(
            new Rate
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Level = RateLevel.Critical,
            },
            new Rate
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Level = RateLevel.High,
            },
            new Rate
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Level = RateLevel.Medium,
            },
            new Rate
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Level = RateLevel.Low,
            },
            new Rate
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Level = RateLevel.Minimal,
            }
        );

    }
}
