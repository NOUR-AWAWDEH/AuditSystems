using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Domain.Entities.Users;

public class CompanyUser : AuditableEntity<Guid>
{
    public required Guid CompanyId { get; set; }
    public required Guid AppUserId { get; set; }
    public required Guid RoleId { get; set; }

    public bool IsActive { get; set; }

    // Navigation
    public virtual Company Company { get; set; } = null!;
    public virtual AppUser AppUser { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}