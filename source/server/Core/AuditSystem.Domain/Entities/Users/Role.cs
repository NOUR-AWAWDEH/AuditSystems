using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Domain.Entities.Users;

public class Role : IdentityRole<Guid>
{
    public const string Admin = "Admin";
    public const string ActionOwner = "ActionOwner";

    public virtual ICollection<User> Users { get; set; } = [];
}