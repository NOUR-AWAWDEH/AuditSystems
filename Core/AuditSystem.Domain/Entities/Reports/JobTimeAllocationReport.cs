using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Reports;

public class JobTimeAllocationReport : Entity<Guid>
{
    public int SerialNumber { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string ReportName { get; set; } = string.Empty;
    public DateOnly ReportDate { get; set; }
    public Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;

    public virtual User Creator { get; set; } = null!; 
}