using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Risks
{
    public class RiskFactor : Entity<Guid>
    {
        public Guid AuditorSettingsID { get; set; }
        public string Factor { get; set; } = string.Empty;

        public AuditorSettings AuditorSettings { get; set; } = null!;
    }
}