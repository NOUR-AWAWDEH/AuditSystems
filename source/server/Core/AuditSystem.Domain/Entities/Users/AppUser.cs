using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Users;

public class AppUser : AuditableEntity<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }

    public string? UserName { get; set; }

    // ✅ REQUIRED FOREIGN KEYS
    public required Guid IdentityUserId { get; set; }
    public required Guid CompanyId { get; set; }
    public required Guid JobTitleId { get; set; }

    public Guid? SubDepartmentId { get; set; }
    public bool IsActive { get; set; }

    // ✅ NAVIGATION PROPERTIES (NOT required)
    public virtual ApplicationUser IdentityUser { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual JobTitle JobTitle { get; set; } = null!;
    public virtual SubDepartment? SubDepartment { get; set; }

    public virtual ICollection<AuditProgram> OwnedAuditPrograms { get; set; } = [];
    public virtual ICollection<UserSessionInfo> UserSessions { get; set; } = [];
}