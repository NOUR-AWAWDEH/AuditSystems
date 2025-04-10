using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Reports;

public class ReportingFollowUp : Entity<Guid>
{
    public required string Reporting { get; set; } = string.Empty;
    public required string FollowUp { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}