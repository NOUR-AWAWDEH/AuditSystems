using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Users;

public class AppUser : AuditableEntity<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }

    // ✅ Identity linkage (correct)
    public required Guid IdentityUserId { get; set; }

    public Guid? JobTitleId { get; set; }
    public Guid? SubDepartmentId { get; set; }

    public bool IsActive { get; set; }

    // ✅ Navigation properties
    public virtual ApplicationUser IdentityUser { get; set; } = null!;
    public virtual JobTitle? JobTitle { get; set; }
    public virtual SubDepartment? SubDepartment { get; set; }

    // ✅ Navigation properties (Many-to-Many via junction table)
    public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = [];

    // ✅ Domain relations
    public virtual ICollection<AuditProgram> OwnedAuditPrograms { get; set; } = [];
    public virtual ICollection<UserSessionInfo> UserSessions { get; set; } = [];
}