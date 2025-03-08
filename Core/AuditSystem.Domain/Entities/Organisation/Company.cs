using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class Company : Entity<Guid>
    {
        public string Name { get; set; } = string.Empty;
    }
}
