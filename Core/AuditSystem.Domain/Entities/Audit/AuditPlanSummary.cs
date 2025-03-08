using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit
{
    public class AuditPlanSummary : Entity<Guid>
    {
        public string Component { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExampleDetails { get; set; } = string.Empty;
    }
}