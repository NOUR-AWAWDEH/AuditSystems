using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Compliance;

public class ComplianceChecklist : Entity<Guid>
{
    public required string Area { get; set; } = string.Empty;
    public required string Subject { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
}