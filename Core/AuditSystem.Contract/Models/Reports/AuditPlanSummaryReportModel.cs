using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Reports;

public sealed class AuditPlanSummaryReportModel : BaseModel<Guid>
{
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
}