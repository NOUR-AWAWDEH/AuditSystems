using AuditSystem.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Primary Key
        builder.HasKey(rt => rt.Id);

        // Properties
        builder.Property(rt => rt.Token)
               .IsRequired()
               .HasMaxLength(2000); // You can adjust the length as needed

        builder.Property(rt => rt.ExpiresAt)
               .IsRequired();

        builder.Property(rt => rt.IsRevoked)
               .IsRequired();

        builder.Property(rt => rt.CreatedAt)
               .IsRequired();

        // Relationship: Many RefreshTokens belong to one User
        builder.HasOne(rt => rt.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade); // Use Cascade or Restrict as per your needs
    }
}
