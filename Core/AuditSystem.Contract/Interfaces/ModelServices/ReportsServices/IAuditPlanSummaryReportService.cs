using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IAuditPlanSummaryReportService
{
    public Task<Guid> CreateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel);
    public Task UpdateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel);
    public Task DeleteAuditPlanSummaryReportAsync(Guid auditPlanSummaryReportId);
    public Task<AuditPlanSummaryReportModel> GetAuditPlanSummaryReportByIdAsync(Guid id);
}