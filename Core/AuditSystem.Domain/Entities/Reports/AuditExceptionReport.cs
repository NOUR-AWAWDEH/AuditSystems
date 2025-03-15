using AuditSystem.Domain.Entities.Users;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Reports;

public class AuditExceptionReport : Entity<Guid>
{
    public int SerialNumber { get; set; } 
    public string ReportName { get;set; } = string.Empty;
    public DateOnly ReportDate { get;set; }
    public Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public virtual User Creator { get; set; } = null!;
}