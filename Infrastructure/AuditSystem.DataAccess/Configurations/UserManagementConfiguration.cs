using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations
{
    public class UserManagementConfiguration : IEntityTypeConfiguration<UserManagement>
    {
        public void Configure(EntityTypeBuilder<UserManagement> builder)
        {
            builder.HasKey(um => um.Id);

            builder.Property(um => um.Designation)
                .IsRequired()
                .HasMaxLength(100);

            // Relationships (Use Restrict or NoAction to avoid cascade paths)
            builder.HasOne(um => um.User)
                .WithMany()
                .HasForeignKey(um => um.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(um => um.AuditorSettings)
                .WithMany()
                .HasForeignKey(um => um.AuditorSettingsID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class AuditorSettingsConfiguration : IEntityTypeConfiguration<AuditorSettings>
    {
        public void Configure(EntityTypeBuilder<AuditorSettings> builder)
        {
            builder.HasKey(a => a.Id);

            // Relationship with User (Prevent cascade delete)
            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
