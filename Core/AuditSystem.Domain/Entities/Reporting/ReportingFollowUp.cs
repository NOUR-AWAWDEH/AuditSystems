using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Reporting;

public class ReportingFollowUp : Entity<Guid>
{
   public string Reporting { get;set; } = string.Empty;
   public string FollowUp { get;set; } = string.Empty;
   public string Status { get;set; } = string.Empty;
}