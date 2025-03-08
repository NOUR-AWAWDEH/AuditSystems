using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users
{
    public class Skill : Entity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}