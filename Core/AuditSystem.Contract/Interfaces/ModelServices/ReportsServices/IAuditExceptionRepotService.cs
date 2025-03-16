using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IAuditExceptionRepotService
{
    public Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel);
}