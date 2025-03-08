using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.RiskControls
{
    public class RiskControl
    {
        public Guid RiskId { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Risk Risk { get; set; } = null!;
    }
}