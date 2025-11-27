using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
       public void Configure(EntityTypeBuilder<AppUser> builder)
       {
              // Table
              builder.ToTable("AppUsers");

              // Primary key
              builder.HasKey(u => u.Id);

              // Required properties
              builder.Property(u => u.FirstName).IsRequired();
              builder.Property(u => u.LastName).IsRequired();
              builder.Property(u => u.Email).IsRequired();
              builder.Property(u => u.UserName).IsRequired();
              builder.Property(u => u.IdentityUserId).IsRequired();
              builder.Property(u => u.CompanyId).IsRequired();
              builder.Property(u => u.JobTitleId).IsRequired();
              builder.Property(u => u.IsActive).IsRequired();

              // Optional
              builder.Property(u => u.SubDepartmentId).IsRequired(false);

              // Relationships
              builder.HasOne(u => u.IdentityUser)
                     .WithMany(iu => iu.AppUsers)
                     .HasForeignKey(u => u.IdentityUserId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasMany(u => u.OwnedAuditPrograms)
                     .WithOne(ap => ap.ActionOwner)
                     .HasForeignKey(ap => ap.ActionOwnerId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(u => u.UserSessions)
                     .WithOne(usi => usi.AppUser)
                     .HasForeignKey(usi => usi.AppUserId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(u => u.Company)
                     .WithMany(c => c.Users)
                     .HasForeignKey(u => u.CompanyId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(u => u.JobTitle)
                     .WithMany()
                     .HasForeignKey(u => u.JobTitleId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(u => u.SubDepartment)
                     .WithMany(sd => sd.Users)
                     .HasForeignKey(u => u.SubDepartmentId)
                     .OnDelete(DeleteBehavior.Restrict);

              // Auditable Entity Relationships
              builder.HasOne(u => u.CreatedBy)
                     .WithMany()
                     .HasForeignKey(u => u.CreatedByUserId)
                     .OnDelete(DeleteBehavior.Restrict);

              // Indexes
              builder.HasIndex(u => u.IdentityUserId).IsUnique();
              builder.HasIndex(u => u.CompanyId);
              builder.HasIndex(u => u.JobTitleId);
              builder.HasIndex(u => u.SubDepartmentId);
              builder.HasIndex(u => u.IsActive);
       }
}
