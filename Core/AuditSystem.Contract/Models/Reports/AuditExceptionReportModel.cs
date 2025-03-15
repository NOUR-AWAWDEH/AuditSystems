using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Reports;

public sealed class AuditExceptionReportModel : BaseModel<Guid>
{
    public int SerialNumber { get; set; }
    public string ReportName { get; set; } = string.Empty;
    public DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
}
