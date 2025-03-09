using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Audit
{
    public class BusinessObjective : Entity<Guid>
    {
        public Guid AuditorSettingsID { get; set; }
        public string Impact { get; set; } = string.Empty;
        public int Amount { get; set; }

        public AuditorSettings AuditorSettings { get; set; } = null!;
    }
}