using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IDriftReportInfoService
{
    Task<DriftReportInfoModel>GenerateReportData(GenerateDriftReportRequest request);
}
