using AuditSystem.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Domain.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public virtual AppUser? AppUser { get; set; }
}
