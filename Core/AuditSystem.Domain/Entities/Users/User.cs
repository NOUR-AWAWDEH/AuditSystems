using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organisation;
using AuditSystem.Domain.Entities.Users;

public class User : Entity<Guid>
{
    public required string UserName { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public Guid? UserRoleId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? SubDepartmentId { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan TotalSessionTime { get; set; }
    public DateTime? CurrentSessionStart { get; set; }

    // Navigation properties
    public virtual UserRole? UserRole { get; set; } = null!;
    public virtual Company? Company { get; set; } = null!;
    public virtual Department? Department { get; set; } = null!;
    public virtual SubDepartment? SubDepartment { get; set; } = null!;
}
