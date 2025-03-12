using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Jobs
{
    public class JobPrioritization : Entity<Guid>
    {
        public int SerialNumber { get; set; }
        public string AuditableUnit { get; set; } = string.Empty;
        public Guid BusinessObjectiveId { get; set; }
        public Guid RatingId { get; set; }
        public bool SelectForAudit { get; set; }
        public string Comments { get; set; } = string.Empty;
        public DateOnly SelectYear { get; set; }

        public virtual Rating Rating { get; set; } = null!;
        public virtual BusinessObjective BusinessObjective { get; set; } = null!;
    }
}