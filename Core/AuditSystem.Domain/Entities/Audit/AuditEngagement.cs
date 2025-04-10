using AuditSystem.Domain.Entities.Common; 

namespace AuditSystem.Domain.Entities.Audit
{
    public class AuditEngagement : Entity<Guid>
    {
        public int SerialNumber { get; set; }
        public string JobName { get; set; } = string.Empty;
        public DateOnly PlannedStartDate { get; set; }
        public DateOnly PlannedEndDate { get; set; }
        public string JobType { get; set; } = string.Empty;
        public string SubLocation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string JobStatus { get; set; } = string.Empty;
    }
}