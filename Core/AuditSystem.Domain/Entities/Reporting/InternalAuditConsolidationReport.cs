using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Reporting;

public class InternalAuditConsolidationReport : Entity<Guid>
{
    public int SerialNumber { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string ReportName { get; set; } = string.Empty;
    public DateOnly ReportDate { get; set; }
    public Guid PreparedByUserId { get; set; } 
    public string Status { get; set; } = string.Empty;

    public virtual User PreparedByUser { get; set; } = null!;
}