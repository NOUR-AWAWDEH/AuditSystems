
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users
{
    public class UserRole : Entity<Guid>
    {
        public const string Admin = "Admin";
        public const string  Auditor = "Auditor";
        public const string User = "User";
    }
}