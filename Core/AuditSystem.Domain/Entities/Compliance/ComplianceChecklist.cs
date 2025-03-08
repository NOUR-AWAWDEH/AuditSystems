using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Compliance
{
    public class ComplianceChecklist : Entity<Guid>
    {
        public int SerialNumber { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Particulars { get; set; } = string.Empty; 
    }
}