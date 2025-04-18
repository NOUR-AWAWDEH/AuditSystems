using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Reports;

public sealed class InternalAuditConsolidationReportModel : BaseModel<Guid>
{
    public required string JobName { get; set; } = string.Empty;
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid PreparedByUserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
