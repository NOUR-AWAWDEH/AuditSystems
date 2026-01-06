using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
{
    public void Configure(EntityTypeBuilder<CompanyUser> builder)
    {
        builder.ToTable("CompanyUsers");

        builder.HasKey(cu => cu.Id);

        builder.HasOne(cu => cu.Company)
               .WithMany(c => c.CompanyUsers)
               .HasForeignKey(cu => cu.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cu => cu.AppUser)
               .WithMany(u => u.CompanyUsers)
               .HasForeignKey(cu => cu.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cu => cu.Role)
               .WithMany()
               .HasForeignKey(cu => cu.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(cu => cu.IsActive)
               .IsRequired()
               .HasDefaultValue(true);

        builder.HasOne(cu => cu.CreatedBy)
               .WithMany()
               .HasForeignKey(cu => cu.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
