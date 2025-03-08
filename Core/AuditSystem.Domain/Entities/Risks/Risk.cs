using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Risks
{
    public class Risk : Entity<Guid>
    {
        public Guid ObjectiveId { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string RiskName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Objective Objective { get; set; } = null!;
    }
}