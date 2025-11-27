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
    public required Guid IdentityUserId { get; set; }
    public required Guid CompanyId { get; set; }
    public Guid? SubDepartmentId { get; set; }
    public Guid JobTitleId { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public virtual required ApplicationUser IdentityUser { get; set; }
    public virtual required Company Company { get; set; }
    public virtual required JobTitle JobTitle { get; set; }
    public virtual SubDepartment? SubDepartment { get; set; }
    public virtual ICollection<AuditProgram> OwnedAuditPrograms { get; set; } = [];
    public virtual ICollection<UserSessionInfo> UserSessions { get; set; } = [];

}
