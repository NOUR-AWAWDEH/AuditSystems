using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Reports;

public class JobTimeAllocationReport : Entity<Guid>
{
    public required string JobName { get; set; } = string.Empty;
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;

    public virtual User Creator { get; set; } = null!;
}