namespace AuditSystem.DataAccess.Configurations;

using AuditSystem.Domain.Entities.Users;
using AuditSystem.Domain.Entities.Organisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary Key (inherited from Entity<Guid>)
        builder.HasKey(u => u.Id);

        // Properties with constraints
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        // Foreign Key Relationships (nullable, optional)
        builder.Property(u => u.UserRoleId)
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
            .HasDefaultValueSql("GETUTCDATE()"); // Default to current UTC time

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.TotalSessionTime)
            .IsRequired()
            .HasDefaultValue(TimeSpan.Zero);

        builder.Property(u => u.CurrentSessionStart)
            .IsRequired(false);

        // Prevent cascade paths by using Restrict
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

        builder.HasOne(u => u.UserRole)
            .WithMany()
            .HasForeignKey(u => u.UserRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(u => u.UserName)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();


        builder.Property(u => u.Password)
            .HasColumnType("nvarchar(100)");
    }
}