using Microsoft.AspNetCore.Identity;
using AuditSystem.Domain.Entities.Organisation;

namespace AuditSystem.Domain.Entities.Users;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public Guid? RoleId { get; set; } // Added back for custom UserRole
    public Guid? CompanyId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? SubDepartmentId { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan TotalSessionTime { get; set; }
    public DateTime? CurrentSessionStart { get; set; }

    public string? RefreshToken { get; set; }
    public string? PasswordResetCode { get; set; }
    public DateTime? PasswordResetCodeExpiration { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiration { get; set; }
    public DateTime? LastLogoutTime { get; set; }

    public virtual Role Role { get; set; } // Added back for custom UserRole
    public virtual Company? Company { get; set; }
    public virtual Department? Department { get; set; }
    public virtual SubDepartment? SubDepartment { get; set; }
}