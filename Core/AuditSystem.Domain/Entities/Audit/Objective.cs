using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Audit
{
    public class Objective : Entity<Guid>
    {
        public Guid RiskControlMatrixId { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public RiskControlMatrix RiskControlMatrix { get; set; } = null!;
    }
}