using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        // Use static GUIDs instead of dynamic ones
        builder.HasData(
            new Role { Id = new Guid("b94a0ed7-4f2a-46a5-a3a9-d2f4d6a4f5c3"), Name = Role.Admin, NormalizedName = Role.Admin.ToUpper() },
            new Role { Id = new Guid("e6a0c0f7-8f9a-4661-b47d-c8a8a37c9d70"), Name = Role.Auditor, NormalizedName = Role.Auditor.ToUpper() },
            new Role { Id = new Guid("92c1a98e-8a3c-4c7e-b9cc-4f8321dcb31d"), Name = Role.User, NormalizedName = Role.User.ToUpper() }
        );
    }
}
