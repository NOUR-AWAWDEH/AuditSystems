using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Domain.Entities.Users;

public class Role : IdentityRole<Guid>
{
    public const string Admin = "Admin";
    public const string Auditor = "Auditor";
    public const string User = "User";
}