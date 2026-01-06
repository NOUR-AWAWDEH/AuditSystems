using System;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(200);


            builder.HasMany(c => c.Locations)
                   .WithOne(l => l.Company)
                   .HasForeignKey(l => l.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
          
            // Seed data

            builder.HasData(
                new Company
                {
                    Id = new Guid("d2719f8e-8b4f-4c0e-a6b8-123456789abc"),
                    Name = "FUTURE-LINKS",
                    CreatedAt = new DateTime(2026, 1, 1, 1, 1, 1, DateTimeKind.Utc)
                }
            );

            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}