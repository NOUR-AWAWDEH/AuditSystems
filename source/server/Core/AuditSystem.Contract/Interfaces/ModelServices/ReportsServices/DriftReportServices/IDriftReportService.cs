using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IDriftReportService
{
    Task<DriftReportModel>GenerateReportData(GenerateDriftReportRequest request);
    Task<DriftReportModel>CreateDriftReportAsync(GenerateDriftReportRequest request);
}
