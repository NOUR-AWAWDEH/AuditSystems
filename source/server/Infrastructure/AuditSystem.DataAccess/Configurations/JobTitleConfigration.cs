using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("JobTitles");

        // Properties
        builder.Property(jt => jt.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(jt => jt.Company)
            .WithMany()
            .HasForeignKey(jt => jt.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(jt => jt.Users)
            .WithOne(u => u.JobTitle)
            .HasForeignKey(u => u.JobTitleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(jt => new { jt.CompanyId, jt.Name })
            .IsUnique();

        // Seed Data
        var companyId = new Guid("d2719f8e-8b4f-4c0e-a6b8-123456789abc"); // FUTURE-LINKS company

        builder.HasData(
            new JobTitle
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "System Administrator",
                CompanyId = companyId,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new JobTitle
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Audit Manager",
                CompanyId = companyId,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new JobTitle
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Senior Auditor",
                CompanyId = companyId,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new JobTitle
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                Name = "Auditor",
                CompanyId = companyId,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new JobTitle
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                Name = "Risk Manager",
                CompanyId = companyId,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new JobTitle
            {
                Id = new Guid("66666666-6666-6666-6666-666666666666"),
                Name = "Compliance Officer",
                CompanyId = companyId,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
