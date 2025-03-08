using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Processes
{
    public class Process : Entity<Guid>
    {
        public Guid RiskControlID { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public RiskControl RiskControl { get; set; } = null!;
    }
}