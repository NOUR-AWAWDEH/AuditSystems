using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name (optional, overrides Identity's default AspNetUsers)
        builder.ToTable("Users");

        // Properties with constraints
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        // Foreign Key Relationships (nullable, optional)
        builder.Property(u => u.RoleId)
            .IsRequired(false);

        builder.Property(u => u.CompanyId)
            .IsRequired(false);

        builder.Property(u => u.DepartmentId)
            .IsRequired(false);

        builder.Property(u => u.SubDepartmentId)
            .IsRequired(false);

        // Other Properties
        builder.Property(u => u.LastLogin)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.TotalSessionTime)
            .IsRequired()
            .HasDefaultValue(TimeSpan.Zero);

        builder.Property(u => u.CurrentSessionStart)
            .IsRequired(false);

        // Identity-related properties (optional)
        builder.Property(u => u.RefreshToken)
            .IsRequired(false);

        builder.Property(u => u.PasswordResetCode)
            .IsRequired(false);

        builder.Property(u => u.PasswordResetCodeExpiration)
            .IsRequired(false);

        builder.Property(u => u.PasswordResetToken)
            .IsRequired(false);

        builder.Property(u => u.PasswordResetTokenExpiration)
            .IsRequired(false);

        builder.Property(u => u.LastLogoutTime)
            .IsRequired(false);

        // Relationships
        builder.HasOne(u => u.Role)
            .WithMany() // No navigation property on UserRole side
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.SubDepartment)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}