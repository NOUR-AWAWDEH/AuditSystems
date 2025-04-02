using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Checklists;

public class Checklist : Entity<Guid>
{
    public string Area { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}