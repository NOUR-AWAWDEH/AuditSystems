using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

public interface IJobSchedulingService
{
    public Task<Guid> CreateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel);
    public Task UpdateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel);
    public Task DeleteJobSchedulingAsync(Guid jobSchedulingId);
    public Task<JobSchedulingModel> GetJobSchedulingByIdAsync(Guid id);
    public Task<PaginationResult<JobSchedulingModel>> SearchAsync(string? name, Page page);
}
