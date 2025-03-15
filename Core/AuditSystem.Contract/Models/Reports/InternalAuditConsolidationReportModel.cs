using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Reports;

public sealed class InternalAuditConsolidationReportModel : BaseModel<Guid>
{
    public int SerialNumber { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string ReportName { get; set; } = string.Empty;
    public DateOnly ReportDate { get; set; }
    public required Guid PreparedByUserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
