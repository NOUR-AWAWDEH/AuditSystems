using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IJobTimeAllocationReportService
{
    public Task<Guid> CreateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel);
    public Task UpdateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel);
    public Task DeleteJobTimeAllocationReportAsync(Guid jobTimeAllocationReportId);
    public Task<JobTimeAllocationReportModel> GetJobTimeAllocationReportByIdAsync(Guid id);
}
