using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Reports;

public sealed class ReportingFollowUpModel : BaseModel<Guid>
{
    public string Reporting { get; set; } = string.Empty;
    public string FollowUp { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}