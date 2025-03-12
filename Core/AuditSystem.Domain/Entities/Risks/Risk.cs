using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Risks
{
    public class Risk : Entity<Guid>
    {
        public Guid ObjectiveId { get; set; }
        public string RiskName { get; set; } = string.Empty;
        public Guid RatingId{ get; set; }
        public string Description { get; set; } = string.Empty;

        public Rating Rating { get; set; } = null!;
        public virtual Objective Objective { get; set; } = null!;
    }
}