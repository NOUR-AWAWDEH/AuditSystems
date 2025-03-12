using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class SubLocation : Entity<Guid>
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual Location Location { get; set; } = null!;
    }
}