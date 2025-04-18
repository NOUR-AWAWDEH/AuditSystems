using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IAuditExceptionReportService
{
    public Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel);
    public Task UpdateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel);
    public Task DeleteAuditExceptionReportAsync(Guid auditExceptionReportId);
    public Task<AuditExceptionReportModel> GetAuditExceptionReportByIdAsync(Guid id);
}