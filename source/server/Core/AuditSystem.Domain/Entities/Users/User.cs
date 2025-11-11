using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Domain.Entities.Users;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;

    public required Guid CompanyId { get; set; }

    public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public TimeSpan TotalSessionTime { get; set; } = TimeSpan.Zero;
    public DateTime? CurrentSessionStart { get; set; }

    public string? PasswordResetToken { get; set; } = null;
    public string? PasswordResetCode { get; set; } = null;
    public DateTime? PasswordResetCodeExpiration { get; set; } = null;
    public DateTime? PasswordResetTokenExpiration { get; set; } = null;
    public DateTime? LastLogoutTime { get; set; } = null;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>();
    public virtual ICollection<SupportingDocs.SupportingDoc> SupportingDocs { get; set; } = new List<SupportingDocs.SupportingDoc>();
    public virtual Company? Company { get; set; } = null!;
}