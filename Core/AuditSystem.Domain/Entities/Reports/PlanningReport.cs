using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Reports;

public class PlanningReport : Entity<Guid>
{
    public int SerialNumber {get; set;}
    public string ReportName {get; set;} = string.Empty;
    public DateOnly ReportDate { get; set; }
    public Guid CreatedById { get;set; }
    public string Status {get; set;} = string.Empty;

    public virtual User Creator { get; set; } = null!;
}