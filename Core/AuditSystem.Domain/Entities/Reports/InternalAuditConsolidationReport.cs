using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Reports;

public class InternalAuditConsolidationReport : Entity<Guid>
{
    public string JobName { get; set; } = string.Empty;
    public string ReportName { get; set; } = string.Empty;
    public DateOnly ReportDate { get; set; }
    public required Guid PreparedByUserId { get; set; } 
    public string Status { get; set; } = string.Empty;

    public virtual User PreparedByUser { get; set; } = null!;
}