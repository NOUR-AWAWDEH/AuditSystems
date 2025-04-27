using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IReportingFollowUpService
{
    public Task<Guid> CreateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel);
    public Task UpdateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel);
    public Task DeleteReportingFollowUpAsync(Guid reportingFollowUpId);
    public Task<ReportingFollowUpModel> GetReportingFollowUpByIdAsync(Guid id);
    public Task<PaginationResult<ReportingFollowUpModel>> SearchAsync(string? name, Page page);
}
