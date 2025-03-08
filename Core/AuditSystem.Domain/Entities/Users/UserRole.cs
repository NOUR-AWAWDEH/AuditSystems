using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users
{
    public class UserRole : Entity<Guid>
    {
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}