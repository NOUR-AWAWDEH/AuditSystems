using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.DataAccess.Configurations;

public class UserConfiguration() : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Required properties
        builder.Property(u => u.FirstName)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(u => u.CompanyId)
            .IsRequired(true);

        // Optional properties with constraints
        builder.Property(u => u.PasswordResetCode)
            .HasMaxLength(100);

        builder.Property(u => u.PasswordResetToken)
            .HasMaxLength(3000);

        // Relationships

        builder.HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.AuditPrograms)
            .WithOne(ap => ap.ActionOwner)
            .HasForeignKey(ap => ap.ActionOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(u => u.CompanyId);
        builder.HasIndex(u => u.LastLogin);
        builder.HasIndex(u => u.IsActive);
    }
}