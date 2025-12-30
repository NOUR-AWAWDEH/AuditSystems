using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IDriftReport
{
    Task<DriftReportData>GenerateReportData(GenerateDriftReportRequest request);
}
