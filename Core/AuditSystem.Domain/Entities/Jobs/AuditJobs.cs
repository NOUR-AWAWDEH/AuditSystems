using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Jobs
{
    public class AuditJobs : Entity<Guid>
    {
        public int AuditUniverseID { get; set; } 
        public int SerialNumber { get; set; }
        public string JobName { get; set; } = string.Empty; 
        public string JobType { get; set; } = string.Empty; 

        public AuditUniverse AuditUniverse { get; set; } = null!;
    }
}