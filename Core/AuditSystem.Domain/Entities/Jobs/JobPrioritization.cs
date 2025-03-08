using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Jobs
{
    public class JobPrioritization
    {
        public int SerialNumber { get; set; }
        public string AuditableUnit { get; set; } = string.Empty;
        public string BusinessObjectiveID { get; set; } = string.Empty;
        public string RiskRating { get; set; } = string.Empty;
        public bool SelectForAudit { get; set; }
        public string Comments { get; set; } = string.Empty;
        public DateOnly SelectYear { get; set; }

        public BusinessObjective BusinessObjective { get; set; } = null!;
    }
}