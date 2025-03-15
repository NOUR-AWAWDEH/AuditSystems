using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

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
