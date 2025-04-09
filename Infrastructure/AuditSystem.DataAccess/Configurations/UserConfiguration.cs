using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Required properties
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.RoleId)
            .IsRequired();

        // Optional properties with constraints
        builder.Property(u => u.PasswordResetCode)
            .HasMaxLength(100);

        builder.Property(u => u.PasswordResetToken)
            .HasMaxLength(200);

        // Relationships
        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Company)
            .WithMany()
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);  // Changed from SetNull to Restrict

        builder.HasOne(u => u.Department)
            .WithMany()
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);  // Changed from SetNull to Restrict

        builder.HasOne(u => u.SubDepartment)
            .WithMany()
            .HasForeignKey(u => u.SubDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);  // Changed from SetNull to Restrict

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(u => u.RoleId);
        builder.HasIndex(u => u.CompanyId);
        builder.HasIndex(u => u.DepartmentId);
        builder.HasIndex(u => u.SubDepartmentId);
        builder.HasIndex(u => u.LastLogin);
        builder.HasIndex(u => u.IsActive);
    }
}