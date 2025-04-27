using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IInternalAuditConsolidationReportService
{
    public Task<Guid> CreateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel);
    public Task UpdateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel);
    public Task DeleteInternalAuditConsolidationReportAsync(Guid internalAuditConsolidationReportId);
    public Task<InternalAuditConsolidationReportModel> GetInternalAuditConsolidationReportByIdAsync(Guid id);
    public Task<PaginationResult<InternalAuditConsolidationReportModel>> SearchAsync(string? name, Page page);
}
